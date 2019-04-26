using ArchsimLib;
using ArchsimLib.LibraryObjects;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace ArchsimLib.CSV
{
   public static class CSVImportExport
    {

        //Generic conversion
        public class MyDoubleListConverter : ITypeConverter
        {
            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {

                return JsonConvert.SerializeObject(value);

            }

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return JsonConvert.DeserializeObject<List<double>>(text);
            }
        }
        public class AutoMap<T> : ClassMap<T>
        {
            public AutoMap()
            {
                var properties = typeof(T).GetProperties();

                // map the name property first
                var nameProperty = properties.FirstOrDefault(p => p.Name == "Name");
                if (nameProperty != null)
                    MapProperty(nameProperty).Index(0);

                foreach (var prop in properties.Where(p => p != nameProperty))
                    MapProperty(prop);
            }

            private MemberMap MapProperty(PropertyInfo pi)
            {
                var map = Map(typeof(T), pi);

                if (typeof(List<double>) == pi.PropertyType)
                {
                    map.TypeConverter<MyDoubleListConverter>();
                }

                // set name
                string name = pi.Name;
                var unitsAttribute = pi.GetCustomAttribute<Units>();
                if (unitsAttribute != null)
                    name = $"{name} {"[" + unitsAttribute.Unit + "]"}";
                map.Name(new string[] { name, pi.Name });

                // set default
                var defaultValueAttribute = pi.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttribute != null)
                    map.Default(defaultValueAttribute.Value);

                return map;
            }
        }
        public static void writeLibCSV<T>(string fp, List<T> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                var csv = new CsvWriter(sw);
                csv.Configuration.RegisterClassMap<AutoMap<T>>();
                csv.WriteRecords(records);
            }
        }
        public static List<T> readLibCSV<T>(string fp)
        {
            var records = new List<T>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                csv.Configuration.RegisterClassMap<AutoMap<T>>();
                records = csv.GetRecords<T>().ToList();
            }
            return records;
        }

        //Special cases
        private static void writeArrayScheduleCSV(string fp, List<ScheduleArray> schedules)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < schedules.Count; i++)
            {
                sb.Append(schedules[i].Name + ((schedules.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);
            for (int i = 0; i < schedules.Count; i++)
            {
                sb.Append(Enum.GetName(typeof(ScheduleType), schedules[i].Type) + ((schedules.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);
            for (int i = 0; i < schedules.Count; i++)
            {
                sb.Append(schedules[i].Category + ((schedules.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);


            for (int j = 0; j < schedules[0].Values.Length; j++)
            {
                for (int i = 0; i < schedules.Count; i++)
                {
                    sb.Append(schedules[i].Values[j] + ((schedules.Count - 1 == i) ? "" : ","));
                }
                sb.Append(System.Environment.NewLine);
            }
            System.IO.File.WriteAllText(fp, sb.ToString());
        }
        private static List<ScheduleArray> readArrayScheduleCSV(string fp)
        {
            var schedules = new List<ScheduleArray>();

            string[] lines = File.ReadAllLines(fp);

            var header = lines[0].Split(',');
            var types = lines[1].Split(',');
            var cats = lines[2].Split(',');

            for (int i = 0; i < header.Length; i++)
            {
                schedules.Add(new ScheduleArray() { Values = new double[8760], Name = header[i], Category = cats[i], Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), types[i]) });
            }

            for (int i = 3; i < lines.Length; i++)
            {
                var lin = lines[i].Split(',');
                for (int j = 0; j < lin.Length; j++)
                {
                    schedules[j].Values[i - 3] = double.Parse(lin[j]);
                }
            }
            return schedules;
        }
        private static void writeYearCSV(string fp, List<YearSchedule> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                var csv = new CsvWriter(sw);
                csv.WriteHeader(typeof(YearSchedule));
                csv.WriteField("Week Schedules Count");
                csv.WriteField("Week Schedules");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteRecord(record.WeekSchedules.Count);
                    foreach (var w in record.WeekSchedules)
                    {
                        csv.WriteField(w.From);
                        csv.WriteField(w.To);
                        foreach (var d in w.Days)
                        {
                            csv.WriteField(d.Name);
                        }
                    }

                    csv.NextRecord();
                }
            }
        }
        private static List<YearSchedule> readYearCSV(string fp, List<DaySchedule> days)
        {
            var records = new List<YearSchedule>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        bool foundAllSchedules = true;
                        YearSchedule record = csv.GetRecord<YearSchedule>();
                        int weekCnt = csv.GetField<int>("Week Schedules Count");
                        int weeksStartAt = csv.GetFieldIndex("Week Schedules Count") + 1;

                        for (int i = weeksStartAt; i < weeksStartAt + weekCnt * 9; i += 9)
                        {
                            var weekSched = new WeekSchedule();

                            weekSched.From = csv.GetField<DateTime>(i);
                            weekSched.To = csv.GetField<DateTime>(i + 1);
                            weekSched.Days = new DaySchedule[7];


                            for (int j = 0; j < 7; j++)
                            {
                                string weekDay = csv.GetField<string>(2 + i + j);

                                if (days.Any(x => x.Name == weekDay))
                                {
                                    weekSched.Days[j] = days.First(x => x.Name == weekDay);
                                }
                                else
                                {
                                    foundAllSchedules = false;
                                }

                            }
                            record.WeekSchedules.Add(weekSched);
                        }

                        if (foundAllSchedules) records.Add(record);
                        else { System.Windows.MessageBox.Show(record.Name + " contains day schedules that are not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }
        private static void writeDayCSV(string fp, List<DaySchedule> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(DaySchedule));
                csv.WriteField("1");
                csv.WriteField("2");
                csv.WriteField("3");
                csv.WriteField("4");
                csv.WriteField("5");
                csv.WriteField("6");
                csv.WriteField("7");
                csv.WriteField("8");
                csv.WriteField("9");
                csv.WriteField("10");
                csv.WriteField("11");
                csv.WriteField("12");
                csv.WriteField("13");
                csv.WriteField("14");
                csv.WriteField("15");
                csv.WriteField("16");
                csv.WriteField("17");
                csv.WriteField("18");
                csv.WriteField("19");
                csv.WriteField("20");
                csv.WriteField("21");
                csv.WriteField("22");
                csv.WriteField("23");
                csv.WriteField("24");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);

                    foreach (var h in record.Values)
                    {
                        csv.WriteField(h);
                    }


                    csv.NextRecord();
                }
            }
        }
        private static List<DaySchedule> readDayCSV(string fp)
        {
            var records = new List<DaySchedule>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        DaySchedule record = csv.GetRecord<DaySchedule>();

                        int layerStartAt = 5;
                        for (int i = layerStartAt; i < layerStartAt + 24; i++)
                        {
                            var h = csv.GetField<double>(i);
                            record.Values.Add(h);
                        }

                        records.Add(record);
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }
        private static void writeOpaqueConstructionsCSV(string fp, List<OpaqueConstruction> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(OpaqueConstruction));
                csv.WriteField("LayerCount");
                csv.WriteField("Layers");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteField(record.Layers.Count);
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Thickness);

                    }
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Material.Name);

                    }

                    csv.NextRecord();
                }
            }
        }
        private static List<OpaqueConstruction> readOpaqueConstructionsCSV(string fp, List<OpaqueMaterial> mat)
        {
            var records = new List<OpaqueConstruction>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        bool foundAllMaterials = true;
                        OpaqueConstruction record = csv.GetRecord<OpaqueConstruction>();
                        int layerCnt = csv.GetField<int>("LayerCount");
                        int layerStartAt = csv.GetFieldIndex("LayerCount") + 1;
                        for (int i = layerStartAt; i < layerStartAt + layerCnt; i++)
                        {
                            var thick = csv.GetField<double>(i);
                            var name = csv.GetField<string>(i + layerCnt);
                            if (mat.Any(x => x.Name == name))
                            {
                                var m = mat.First(x => x.Name == name);
                                record.Layers.Add(new Layer<OpaqueMaterial>(thick, m));
                            }
                            else { foundAllMaterials = false; }

                        }
                        if (foundAllMaterials) records.Add(record);
                        else { System.Windows.MessageBox.Show(record.Name + " contains materials not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }
        private static void writeGlazingConstructionsCSV(string fp, List<GlazingConstruction> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(GlazingConstruction));
                csv.WriteField("LayerCount");
                csv.WriteField("Layers");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteField(record.Layers.Count);
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Thickness);

                    }
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Material.Name);

                    }

                    csv.NextRecord();
                }
            }
        }
        private static List<GlazingConstruction> readGlazingConstructionsCSV(string fp, List<GlazingMaterial> mat, List<GasMaterial> gas)
        {
            var records = new List<GlazingConstruction>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        bool foundAllMaterials = true;
                        GlazingConstruction record = csv.GetRecord<GlazingConstruction>();
                        int layerCnt = csv.GetField<int>("LayerCount");
                        int layerStartAt = csv.GetFieldIndex("LayerCount") + 1;
                        for (int i = layerStartAt; i < layerStartAt + layerCnt; i++)
                        {
                            var thick = csv.GetField<double>(i);
                            var name = csv.GetField<string>(i + layerCnt);
                            if (mat.Any(x => x.Name == name))
                            {
                                var m = mat.First(x => x.Name == name);
                                record.Layers.Add(new Layer<WindowMaterialBase>(thick, m));
                            }
                            else if (gas.Any(x => x.Name == name))
                            {
                                var thegas = gas.First(x => x.Name == name);
                                record.Layers.Add(new Layer<WindowMaterialBase>(thick, thegas));
                            }
                            else { foundAllMaterials = false; }

                        }
                        if (foundAllMaterials) records.Add(record);
                        else { System.Windows.MessageBox.Show(record.Name + " contains materials not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }


        private static void writeZoneDefCSV(string fp, List<ZoneDefinition> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                var csv = new CsvWriter(sw);
              
                csv.WriteField("Name");
                csv.WriteField("Loads");
                csv.WriteField("Conditioning");
                csv.WriteField("Ventilation");
                csv.WriteField("HotWater");
                csv.WriteField("Constructions");

                csv.NextRecord();

                foreach (var record in records)
                {
                    csv.WriteField(record.Name);
                    csv.WriteField(record.Loads.Name);
                    csv.WriteField(record.Conditioning.Name);
                    csv.WriteField(record.Ventilation.Name);
                    csv.WriteField(record.DomHotWater.Name);
                    csv.WriteField(record.Materials.Name);
               
                    csv.NextRecord();

                }

            }
        }
        private static List<ZoneDefinition> readZoneDefCSV(string fp, Library Lib)
        {
            var records = new List<ZoneDefinition>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    try
                    {
                        ZoneDefinition record = new ZoneDefinition();
                        record.Name = csv.GetField<string>("Name");

                        string loads = csv.GetField<string>("Loads");
                        string cond = csv.GetField<string>("Conditioning");
                        string vent = csv.GetField<string>("Ventilation");
                        string hotwater = csv.GetField<string>("HotWater");
                        string constructions = csv.GetField<string>("Constructions");

                        record.Loads = Lib.ZoneLoads.First(x => x.Name == loads);
                        record.Conditioning = Lib.ZoneConditionings.First(x => x.Name == cond);
                        record.Ventilation = Lib.ZoneVentilations.First(x => x.Name == vent);
                        record.DomHotWater = Lib.DomHotWaters.First(x => x.Name == hotwater);
                        record.Materials = Lib.ZoneConstructions.First(x => x.Name == constructions);

                        records.Add(record);
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }


        public static void ExportLibrary(Library lib, string folderPath)
        {
            folderPath += @"\ArchsimLibrary-" + lib.TimeStamp.Year + "-" + lib.TimeStamp.Month + "-" + lib.TimeStamp.Day + "-" + lib.TimeStamp.Hour + "-" + lib.TimeStamp.Minute;

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);


            //Schedules
            writeDayCSV(folderPath + @"\DaySchedules.csv", lib.DaySchedules.ToList());

            writeYearCSV(folderPath + @"\YearSchedules.csv", lib.YearSchedules.ToList());

            writeArrayScheduleCSV(folderPath + @"\ArraySchedules.csv", lib.ArraySchedules.ToList());


            //Material Construction
            writeLibCSV<OpaqueMaterial>(folderPath + @"\OpaqueMaterials.csv", lib.OpaqueMaterials.ToList());

            writeOpaqueConstructionsCSV(folderPath + @"\OpaqueConstructions.csv", lib.OpaqueConstructions.ToList());

            //Glazing
            writeLibCSV<GlazingMaterial>(folderPath + @"\GlazingMaterials.csv", lib.GlazingMaterials.ToList());


            writeGlazingConstructionsCSV(folderPath + @"\GlazingConstructions.csv", lib.GlazingConstructions.ToList());


            writeLibCSV<GlazingConstructionSimple>(folderPath + @"\GlazingConstructionSimple.csv", lib.GlazingConstructionsSimple.ToList());


            //Settings

            writeLibCSV<WindowSettings>(folderPath + @"\WindowSettings.csv", lib.WindowSettings.ToList());

            writeLibCSV<ZoneVentilation>(folderPath + @"\ZoneVentilations.csv", lib.ZoneVentilations.ToList());

            writeLibCSV<ZoneConditioning>(folderPath + @"\ZoneConditioning.csv", lib.ZoneConditionings.ToList());

            writeLibCSV<ZoneConstruction>(folderPath + @"\ZoneConstruction.csv", lib.ZoneConstructions.ToList());

            writeLibCSV<ZoneLoad>(folderPath + @"\ZoneLoad.csv", lib.ZoneLoads.ToList());

            writeLibCSV<DomHotWater>(folderPath + @"\DomHotWater.csv", lib.DomHotWaters.ToList());

            // writeLibCSV<ZoneDefinition>(folderPath + @"\ZoneDefinition.csv", lib.ZoneDefinitions.ToList());
            writeZoneDefCSV(folderPath + @"\ZoneDefinition.csv", lib.ZoneDefinitions.ToList());
        }

        public static Library ImportLibrary(string folderPath)
        {

            Library lib = LibraryDefaults.getHardCodedDefaultLib();

            //Schedules
            List<DaySchedule> inDaySchedules = readDayCSV(folderPath + @"\DaySchedules.csv");
            lib.DaySchedules = inDaySchedules;

            List<YearSchedule> inYearSchedules = readYearCSV(folderPath + @"\YearSchedules.csv", inDaySchedules);
            lib.YearSchedules = inYearSchedules;

            List<ScheduleArray> inArraySchedules = readArrayScheduleCSV(folderPath + @"\ArraySchedules.csv");
            lib.ArraySchedules = inArraySchedules;

            //Material Construction
            List<OpaqueMaterial> inOMat = readLibCSV<OpaqueMaterial>(folderPath + @"\OpaqueMaterials.csv");
            lib.OpaqueMaterials = inOMat;

            List<OpaqueConstruction> inOpaqueConstructions = readOpaqueConstructionsCSV(folderPath + @"\OpaqueConstructions.csv", inOMat);
            lib.OpaqueConstructions = inOpaqueConstructions;

            //Glazing
            List<GlazingMaterial> inGMat = readLibCSV<GlazingMaterial>(folderPath + @"\GlazingMaterials.csv");
            lib.GlazingMaterials = inGMat;

            List<GlazingConstruction> inGlazingConstructions = readGlazingConstructionsCSV(folderPath + @"\GlazingConstructions.csv", lib.GlazingMaterials.ToList(), lib.GasMaterials.ToList());
            lib.GlazingConstructions = inGlazingConstructions;

            List<GlazingConstructionSimple> inGlazingConstructionSimple = readLibCSV<GlazingConstructionSimple>(folderPath + @"\GlazingConstructionSimple.csv");
            lib.GlazingConstructionsSimple = inGlazingConstructionSimple;

            //Settings

            List<WindowSettings> inWindowSettings = readLibCSV<WindowSettings>(folderPath + @"\WindowSettings.csv");
            lib.WindowSettings = inWindowSettings;

            List<ZoneVentilation> inZoneVentilation = readLibCSV<ZoneVentilation>(folderPath + @"\ZoneVentilations.csv");
            lib.ZoneVentilations = inZoneVentilation;

            List<ZoneConditioning> inZoneConditioning = readLibCSV<ZoneConditioning>(folderPath + @"\ZoneConditioning.csv");
            lib.ZoneConditionings = inZoneConditioning;

            List<ZoneConstruction> inZoneConstruction = readLibCSV<ZoneConstruction>(folderPath + @"\ZoneConstruction.csv");
            lib.ZoneConstructions = inZoneConstruction;

            List<ZoneLoad> inZoneLoad = readLibCSV<ZoneLoad>(folderPath + @"\ZoneLoad.csv");
            lib.ZoneLoads = inZoneLoad;

            List<DomHotWater> inDomHotWater = readLibCSV<DomHotWater>(folderPath + @"\DomHotWater.csv");
            lib.DomHotWaters = inDomHotWater;

            //List<ZoneDefinition> inZoneDefinition = readLibCSV<ZoneDefinition>(folderPath + @"\ZoneDefinition.csv");
            //lib.ZoneDefinitions = inZoneDefinition;

            List<ZoneDefinition> inZoneDefinition = readZoneDefCSV(folderPath + @"\ZoneDefinition.csv", lib);
            lib.ZoneDefinitions = inZoneDefinition;



            var defaultLib = LibraryDefaults.getHardCodedDefaultLib();
            defaultLib.Merge(lib);



            return lib;
        }



    }
}
