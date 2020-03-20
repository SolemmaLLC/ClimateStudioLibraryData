using CSEnergyLib;
using CSEnergyLib.LibraryObjects;
using CSEnergyLib.Utilities;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace CSEnergyLib.CSV
{
   public class CSVImportExport
    {

        //Special Type Converters
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

        public class MyGlazingLayerConverter : ITypeConverter
        {
            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                string s = "[";
                var layers = value as List<CSWindowMaterialBase>;
                foreach (var l in layers) {
                    s += l.Name + ", ";
                }
                s = s.Trim();
                s = s.Remove(s.Length - 1, 1);
                s += "]";
                return s;
            }

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (String.IsNullOrWhiteSpace(text)) return null;

                text = text.Trim();
                text = text.Remove(0,1);
                text = text.Remove(text.Length - 1, 1);

                string[] nameArr = text.Split(',');
                 List<CSWindowMaterialBase> layers = new List<CSWindowMaterialBase>();

                for (int i = 0; i < nameArr.Length; i++)
                {
                    //var thick = csv.GetField<double>(i);
                    var name = nameArr[i].Trim();  

                    if (newLib.GlazingMaterials.Any(x => x.Name == name))
                    {
                        var m = newLib.GlazingMaterials.First(x => x.Name == name);
                        layers.Add(m);
                    }
                    else if (newLib.GasMaterials.Any(x => x.Name == name))
                    {
                        var thegas = newLib.GasMaterials.First(x => x.Name == name);
                        layers.Add(thegas);
                    }
                    else {
                        System.Windows.MessageBox.Show("Referenced object " + nameArr[i] + " not found in library");
                     }

                }
  



                return layers;
            }
        }

        public class MyOpaqueLayerConverter : ITypeConverter
        {
            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                string s = "[";
                var layers = value as List<Layer<CSOpaqueMaterial>>;
                foreach (var l in layers)
                {
                    s += l.Material.Name + ", " + l.Thickness +", ";
                }
                s = s.Trim();
                s = s.Remove(s.Length - 1, 1);
                s += "]";
                return s;
            }

            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                if (String.IsNullOrWhiteSpace(text)) return null;

                text = text.Trim();
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1, 1);

                string[] nameArr = text.Split(',');
                List<Layer<CSOpaqueMaterial>> layers = new List<Layer<CSOpaqueMaterial>>();

                for (int i = 0; i < nameArr.Length; i+=2)
                {
                    //var thick = csv.GetField<double>(i);
                    var name = nameArr[i].Trim(); 
                    var thick = double.Parse(nameArr[i+1]);

                    if (newLib.OpaqueMaterials.Any(x => x.Name.ToUpper() == name.ToUpper()))
                    {
                        var m = newLib.OpaqueMaterials.First(x => x.Name == name);
                        layers.Add(new Layer<CSOpaqueMaterial>(thick, m));
                    }
                  
                    else
                    {
                        System.Windows.MessageBox.Show("Referenced object " + nameArr[i] + " not found in library");
                    }

                }




                return layers;
            }
        }


        //Custom Mapper

        public class AutoMap<T> : ClassMap<T>
        {
            public AutoMap()
            {
                var properties = typeof(T).GetProperties();
                
                // map the name property first
                var nameProperty = properties.FirstOrDefault(p => p.Name == "Name"); 
                if (nameProperty != null) MapProperty(nameProperty).Index(0);

                var isLockedProperty = properties.FirstOrDefault(p => p.Name == "IsLocked");
                var libNameProperty = properties.FirstOrDefault(p => p.Name == "LibraryName");
                var defaultProperty = properties.FirstOrDefault(p => p.Name == "IsDefault");


                if (isLockedProperty != null) MapProperty(isLockedProperty).Ignore();
                if (libNameProperty != null) MapProperty(libNameProperty).Ignore();
                if (defaultProperty != null) MapProperty(defaultProperty).Ignore();

                foreach (var prop in properties.Where(p => p != nameProperty && p != isLockedProperty && p != libNameProperty && p != defaultProperty ))
                {
                    MapProperty(prop);
                }

               

            }

            private MemberMap MapProperty(PropertyInfo pi)
            {
                var map = Map(typeof(T), pi);

                if (typeof(List<double>) == pi.PropertyType)
                {
                    map.TypeConverter<MyDoubleListConverter>();
                }
                else if (typeof(List<CSWindowMaterialBase>) == pi.PropertyType)
                {
                    map.TypeConverter<MyGlazingLayerConverter>();
                }
                else if (typeof(List<Layer<CSOpaqueMaterial>>) == pi.PropertyType)
                {
                    map.TypeConverter<MyOpaqueLayerConverter>();
                }


                // set name
                string name = pi.Name;
                var unitsAttribute = pi.GetCustomAttribute<Units>();
                if (unitsAttribute != null)
                {
                    name = $"{name} {"[" + unitsAttribute.Unit + "]"}";
                }
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
            if (records.Count == 0) return;
            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<AutoMap<T>>();

                csv.WriteRecords(records);
            }

        }
        public static List<T> readLibCSV<T>(string fp)
        {
            try
            {
                var records = new List<T>();

                if (!File.Exists(fp))
                {
                    //Eto.Forms.MessageBox.Show(fp + " is missing!");
                    return records;
                }

                using (var reader = new StreamReader(fp))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.IgnoreBlankLines = true;


                    csv.Configuration.RegisterClassMap<AutoMap<T>>();
                    records = csv.GetRecords<T>().ToList();
                }
                return records;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return new List<T>();
            }
        }

        //Special cases
        private static void writeArrayScheduleCSV(string fp, List<CSArraySchedule> records)
        {
            if (records.Count == 0) return ;


            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < records.Count; i++)
            {
                sb.Append(records[i].Name + ((records.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);
            for (int i = 0; i < records.Count; i++)
            {
                sb.Append(Enum.GetName(typeof(ScheduleType), records[i].Type) + ((records.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);
            for (int i = 0; i < records.Count; i++)
            {
                sb.Append(records[i].Category + ((records.Count - 1 == i) ? "" : ","));
            }
            sb.Append(System.Environment.NewLine);


            for (int j = 0; j < records[0].Values.Length; j++)
            {
                for (int i = 0; i < records.Count; i++)
                {
                    sb.Append(records[i].Values[j] + ((records.Count - 1 == i) ? "" : ","));
                }
                sb.Append(System.Environment.NewLine);
            }
            System.IO.File.WriteAllText(fp, sb.ToString());
        }
        private static List<CSArraySchedule> readArrayScheduleCSV(string fp)
        {
            var schedules = new List<CSArraySchedule>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return schedules;
            }

            string[] lines = File.ReadAllLines(fp);

            var header = lines[0].Split(',');
            var types = lines[1].Split(',');
            var cats = lines[2].Split(',');
            
            for (int i = 0; i < header.Length; i++)
            {
                var type = (ScheduleType)Enum.Parse(typeof(ScheduleType), types[i]);
                schedules.Add(new CSArraySchedule(new double[8760], type) {    Name = header[i], Category = cats[i]  });
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
        private static void writeYearCSV(string fp, List<CSYearSchedule> records)
        {
            if (records.Count == 0) return;


            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader(typeof(CSYearSchedule));
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
        private static List<CSYearSchedule> readYearCSV(string fp, List<CSDaySchedule> days)
        {
            var records = new List<CSYearSchedule>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return records;
            }


            using (var reader = new StreamReader(fp))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;

                while (csv.Read())
                {
                    try
                    {
                        bool foundAllSchedules = true;
                        CSYearSchedule record = csv.GetRecord<CSYearSchedule>();
                        if (record == null) continue;
                        int weekCnt = csv.GetField<int>("Week Schedules Count");
                        int weeksStartAt = csv.GetFieldIndex("Week Schedules Count") + 1;

                        for (int i = weeksStartAt; i < weeksStartAt + weekCnt * 9; i += 9)
                        {
                            var weekSched = new WeekSchedule();

                            weekSched.From = csv.GetField<DateTime>(i);
                            weekSched.To = csv.GetField<DateTime>(i + 1);
                            weekSched.Days = new CSDaySchedule[7];


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

                        record.Total = record.LoadHours();

                        if (foundAllSchedules) records.Add(record);
                        //else { Eto.Forms.MessageBox.Show(record.Name + " contains day schedules that are not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }
        [Obsolete]
        private static void writeDayCSV(string fp, List<CSDaySchedule> records)
        {
            if (records.Count == 0) return;

            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
 
                csv.WriteHeader(typeof(CSDaySchedule));
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
        [Obsolete]
        private static List<CSDaySchedule> readDayCSV(string fp)
        {
            var records = new List<CSDaySchedule>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return records;
            }


            using (var reader = new StreamReader(fp))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;

                while (csv.Read())
                    {
                        try
                        {
                            CSDaySchedule record = csv.GetRecord<CSDaySchedule>();
                            if (record == null) continue;
                            int layerStartAt = 5;
                            for (int i = layerStartAt; i < layerStartAt + 24; i++)
                            {
                                var h = csv.GetField<double>(i);
                                record.Values.Add(h);
                            }
                            record.Total = record.Values.Sum();
                            records.Add(record);
                        }
                        catch (Exception ex) { Debug.WriteLine(ex.Message); }
                    }
                }
                return records;

            }




        [Obsolete]
        private static void writeOpaqueConstructionsCSV(string fp, List<CSOpaqueConstruction> records)
        {
            if (records.Count == 0) return;

            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<AutoMap<CSOpaqueConstruction>>();

                csv.WriteHeader(typeof(CSOpaqueConstruction));
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
        [Obsolete]
        private static List<CSOpaqueConstruction> readOpaqueConstructionsCSV(string fp, List<CSOpaqueMaterial> mat)
        {
            var records = new List<CSOpaqueConstruction>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return records;
            }


            using (var reader = new StreamReader(fp))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;

                while (csv.Read())
                {
                    try
                    {
                        bool foundAllMaterials = true;
                        CSOpaqueConstruction record = csv.GetRecord<CSOpaqueConstruction>();
                        if (record == null) continue;

                        int layerCnt = csv.GetField<int>("LayerCount");
                        int layerStartAt = csv.GetFieldIndex("LayerCount") + 1;
                        for (int i = layerStartAt; i < layerStartAt + layerCnt; i++)
                        {
                            var thick = csv.GetField<double>(i);
                            var name = csv.GetField<string>(i + layerCnt);
                            if (mat.Any(x => x.Name == name))
                            {
                                var m = mat.First(x => x.Name == name);
                                record.Layers.Add(new Layer<CSOpaqueMaterial>(thick, m));
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
        [Obsolete]
        private static void writeGlazingConstructionsCSV(string fp, List<CSGlazingConstruction> records)
        {
            if (records.Count == 0) return;

            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
             
                csv.WriteHeader(typeof(CSGlazingConstruction));
                csv.WriteField("LayerCount");
                csv.WriteField("Layers");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteField(record.Layers.Count);
                    //foreach (var l in record.Layers)
                    //{
                    //    csv.WriteField(l.Thickness);
                    //}
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Name);

                    }

                    csv.NextRecord();
                }
            }
        }
        [Obsolete]
        private static List<CSGlazingConstruction> readGlazingConstructionsCSV(string fp, List<CSGlazingMaterial> mat, List<CSGasMaterial> gas)
        {
            var records = new List<CSGlazingConstruction>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return records;
            }


            using (var reader = new StreamReader(fp))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;

                while (csv.Read())
                {
                    try
                    {
                        bool foundAllMaterials = true;
                        CSGlazingConstruction record = csv.GetRecord<CSGlazingConstruction>();

                        if (record == null) continue;

                        int layerCnt = csv.GetField<int>("LayerCount");
                        int layerStartAt = csv.GetFieldIndex("LayerCount") + 1;
                        for (int i = layerStartAt; i < layerStartAt + layerCnt; i++)
                        {
                            //var thick = csv.GetField<double>(i);
                            var name = csv.GetField<string>(i);
                            if (mat.Any(x => x.Name == name))
                            {
                                var m = mat.First(x => x.Name == name);
                                record.Layers.Add(m);
                            }
                            else if (gas.Any(x => x.Name == name))
                            {
                                var thegas = gas.First(x => x.Name == name);
                                record.Layers.Add(thegas);
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





        private static void writeZoneDefCSV(string fp, List<CSZoneDefinition> records)
        {
            if (records.Count == 0) return;


            using (var writer = new StreamWriter(fp))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
  

                csv.WriteField("Name");
                csv.WriteField("Loads");
                csv.WriteField("Conditioning");
                csv.WriteField("Ventilation");
                csv.WriteField("HotWater");
                csv.WriteField("Constructions");

                csv.WriteField("HeatingCO2 [Kg/kWh]");
                csv.WriteField("HeatingCost [$/kWh]");
                csv.WriteField("CoolingCO2 [Kg/kWh]");
                csv.WriteField("CoolingCost [$/kWh]");
                csv.WriteField("HowWaterCO2 [Kg/kWh]");
                csv.WriteField("HotWaterCost [$/kWh]");
                csv.WriteField("ElectricityCO2 [Kg/kWh]");
                csv.WriteField("ElectricityCost [$/kWh]");

                csv.WriteField("Data Source");

                csv.NextRecord();

                foreach (var record in records)
                {
                    csv.WriteField(record.Name);
                    csv.WriteField(record.Loads.Name);
                    csv.WriteField(record.Conditioning.Name);
                    csv.WriteField(record.Ventilation.Name);
                    csv.WriteField(record.DomHotWater.Name);
                    csv.WriteField(record.Materials.Name);

                    csv.WriteField(record.HeatingCO2);
                    csv.WriteField(record.HeatingCost);
                    csv.WriteField(record.CoolingCO2);
                    csv.WriteField(record.CoolingCost);
                    csv.WriteField(record.HotWaterCO2);
                    csv.WriteField(record.HotWaterCost);
                    csv.WriteField(record.ElectricityCO2);
                    csv.WriteField(record.ElectricityCost);

                    csv.WriteField(record.DataSource);

                    csv.NextRecord();

                }

            }
        }
          
        private static List<CSZoneDefinition> readZoneDefCSV(string fp, CSLibrary Lib)
        {
            var records = new List<CSZoneDefinition>();

            if (!File.Exists(fp))
            {
                //Eto.Forms.MessageBox.Show(fp + " is missing!");
                return records;
            }


            using (var reader = new StreamReader(fp))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.IgnoreBlankLines = true;


                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    try
                    {
                        CSZoneDefinition record = new CSZoneDefinition();
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

                        record.HeatingCO2 = csv.GetField<double>("HeatingCO2 [Kg/kWh]");
                        record.HeatingCost = csv.GetField<double>("HeatingCost [$/kWh]");
                        record.CoolingCO2 = csv.GetField<double>("CoolingCO2 [Kg/kWh]");
                        record.CoolingCost = csv.GetField<double>("CoolingCost [$/kWh]");
                        record.HotWaterCO2 = csv.GetField<double>("HowWaterCO2 [Kg/kWh]");
                        record.HotWaterCost = csv.GetField<double>("HotWaterCost [$/kWh]");
                        record.ElectricityCO2 = csv.GetField<double>("ElectricityCO2 [Kg/kWh]");
                        record.ElectricityCost = csv.GetField<double>("ElectricityCost [$/kWh]");

                        string datasource = csv.GetField<string>("Data Source");
                        record.DataSource = datasource;

                        records.Add(record);
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }







        public static void ExportLibrary(CSLibrary lib, string folderPath)
        {
            //folderPath += @"\ClimateStudioLibrary-" + lib.TimeStamp.Year + "-" + lib.TimeStamp.Month + "-" + lib.TimeStamp.Day + "-" + lib.TimeStamp.Hour + "-" + lib.TimeStamp.Minute;

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);


            //Schedules
            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\DaySchedules_Old.csv"))){
                writeDayCSV(folderPath + @"\DaySchedules_Old.csv", lib.DaySchedules.ToList());
            }
            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\DaySchedules.csv")))
            {
                writeLibCSV<CSDaySchedule>(folderPath + @"\DaySchedules.csv", lib.DaySchedules.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\YearSchedules.csv")))
            {
                writeYearCSV(folderPath + @"\YearSchedules.csv", lib.YearSchedules.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ArraySchedules.csv")))
            {
                writeArrayScheduleCSV(folderPath + @"\ArraySchedules.csv", lib.ArraySchedules.ToList());
            }

            //Material Construction

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\OpaqueMaterials.csv")))
            {
                writeLibCSV<CSOpaqueMaterial>(folderPath + @"\OpaqueMaterials.csv", lib.OpaqueMaterials.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\OpaqueConstructions_Old.csv")))
            {
                writeOpaqueConstructionsCSV(folderPath + @"\OpaqueConstructions_Old.csv", lib.OpaqueConstructions.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\OpaqueConstructions.csv")))
            {
                writeLibCSV<CSOpaqueConstruction>(folderPath + @"\OpaqueConstructions.csv", lib.OpaqueConstructions.ToList());
            }


            //Glazing
            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\GlazingMaterials.csv")))
            {
                writeLibCSV<CSGlazingMaterial>(folderPath + @"\GlazingMaterials.csv", lib.GlazingMaterials.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\GasMaterials.csv")))
            {
                writeLibCSV<CSGasMaterial>(folderPath + @"\GasMaterials.csv", lib.GasMaterials.ToList());
            }


            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\GlazingConstructions_Old.csv")))
            {
                writeGlazingConstructionsCSV(folderPath + @"\GlazingConstructions_Old.csv", lib.GlazingConstructions.ToList());
            }


            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\GlazingConstructions.csv")))
            {
                writeLibCSV<CSGlazingConstruction>(folderPath + @"\GlazingConstructions.csv", lib.GlazingConstructions.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\GlazingConstructionSimple.csv")))
            {
                writeLibCSV<CSGlazingConstructionSimple>(folderPath + @"\GlazingConstructionSimple.csv", lib.GlazingConstructionsSimple.ToList());
            }

            //Settings
            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\WindowSettings.csv")))
            {
                writeLibCSV<CSWindowDefinition>(folderPath + @"\WindowSettings.csv", lib.WindowSettings.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ZoneVentilations.csv")))
            {
                writeLibCSV<CSZoneVentilation>(folderPath + @"\ZoneVentilations.csv", lib.ZoneVentilations.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ZoneConditioning.csv")))
            {
                writeLibCSV<CSZoneConditioning>(folderPath + @"\ZoneConditioning.csv", lib.ZoneConditionings.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ZoneConstruction.csv")))
            {
                writeLibCSV<CSZoneConstruction>(folderPath + @"\ZoneConstruction.csv", lib.ZoneConstructions.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ZoneLoad.csv")))
            {
                writeLibCSV<CSZoneLoad>(folderPath + @"\ZoneLoad.csv", lib.ZoneLoads.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\DomHotWater.csv")))
            {
                writeLibCSV<CSZoneHotWater>(folderPath + @"\DomHotWater.csv", lib.DomHotWaters.ToList());
            }

            if (!FilesAndFolders.IsFileLocked(new FileInfo(folderPath + @"\ZoneDefinition.csv")))
            {
                // writeLibCSV<ZoneDefinition>(folderPath + @"\ZoneDefinition.csv", lib.ZoneDefinitions.ToList());
                writeZoneDefCSV(folderPath + @"\ZoneDefinition.csv", lib.ZoneDefinitions.ToList());
            }

            
        }


       public static CSLibrary newLib = new CSLibrary();
        public static CSLibrary ImportLibrary(string folderPath)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();

            newLib.Clear();
            //newLib = LibraryDefaults.GetMinimumRequiredBaseLibrary();

            //Schedules
            //if (File.Exists(folderPath + @"\DaySchedules.csv"))
            //{
            //    List<CSDaySchedule> inDaySchedules = readDayCSV(folderPath + @"\DaySchedules.csv");
            //    newLib.AddRange(inDaySchedules);
            //}
            if (File.Exists(folderPath + @"\DaySchedules.csv"))
            {
                List<CSDaySchedule> inDaySchedules = readLibCSV<CSDaySchedule>(folderPath + @"\DaySchedules.csv");
                newLib.AddRange(inDaySchedules);
            }
            if (File.Exists(folderPath + @"\YearSchedules.csv"))
            {
                List<CSYearSchedule> inYearSchedules = readYearCSV(folderPath + @"\YearSchedules.csv", newLib.DaySchedules);
                newLib.AddRange(inYearSchedules);
            }

            if (File.Exists(folderPath + @"\ArraySchedules.csv"))
            {
                List<CSArraySchedule> inArraySchedules = readArrayScheduleCSV(folderPath + @"\ArraySchedules.csv");
                newLib.AddRange(inArraySchedules);
            }

            //Opaque
            if (File.Exists(folderPath + @"\OpaqueMaterials.csv"))
            {
                List<CSOpaqueMaterial> inOMat = readLibCSV<CSOpaqueMaterial>(folderPath + @"\OpaqueMaterials.csv");
                newLib.AddRange(inOMat);
            }
            
            if (File.Exists(folderPath + @"\OpaqueConstructions_Old.csv"))
            {
                List<CSOpaqueConstruction> inOpaqueConstructions_Old = readOpaqueConstructionsCSV(folderPath + @"\OpaqueConstructions_Old.csv", newLib.OpaqueMaterials);
                newLib.AddRange(inOpaqueConstructions_Old);
            }

            if (File.Exists(folderPath + @"\OpaqueConstructions.csv"))
            {
                List<CSOpaqueConstruction> inOpaqueConstructions = readLibCSV<CSOpaqueConstruction>(folderPath + @"\OpaqueConstructions.csv");
                newLib.AddRange(inOpaqueConstructions);
            }

            //Glazing
            if (File.Exists(folderPath + @"\GlazingMaterials.csv"))
            {
                List<CSGlazingMaterial> inGMat = readLibCSV<CSGlazingMaterial>(folderPath + @"\GlazingMaterials.csv");
                newLib.AddRange(inGMat);
             }

            if (File.Exists(folderPath + @"\GasMaterials.csv"))
            {
                List<CSGasMaterial> inGasMat = readLibCSV<CSGasMaterial>(folderPath + @"\GasMaterials.csv");
                newLib.AddRange(inGasMat);
             }

            if (File.Exists(folderPath + @"\GlazingConstructions_Old.csv"))
            {
                List<CSGlazingConstruction> inGlazingConstructions_OLD = readGlazingConstructionsCSV(folderPath + @"\OpaqueConstructions_Old.csv", newLib.GlazingMaterials.ToList(), newLib.GasMaterials.ToList());
                newLib.AddRange( inGlazingConstructions_OLD);
            }

            if (File.Exists(folderPath + @"\GlazingConstructions.csv"))
            {
                List<CSGlazingConstruction> inGlazingConstructions = readLibCSV<CSGlazingConstruction>(folderPath + @"\GlazingConstructions.csv");
                newLib.AddRange(inGlazingConstructions);
            }

            if (File.Exists(folderPath + @"\GlazingConstructionSimple.csv"))
            {
                List<CSGlazingConstructionSimple> inGlazingConstructionSimple = readLibCSV<CSGlazingConstructionSimple>(folderPath + @"\GlazingConstructionSimple.csv");
                newLib.AddRange(inGlazingConstructionSimple);
            }




            //Settings
            if (File.Exists(folderPath + @"\WindowSettings.csv"))
            {
                List<CSWindowDefinition> inWindowSettings = readLibCSV<CSWindowDefinition>(folderPath + @"\WindowSettings.csv");
                newLib.AddRange(inWindowSettings);
            }
            if (File.Exists(folderPath + @"\ZoneVentilations.csv"))
            {
                List<CSZoneVentilation> inZoneVentilation = readLibCSV<CSZoneVentilation>(folderPath + @"\ZoneVentilations.csv");
                newLib.AddRange(inZoneVentilation);
            }
            if (File.Exists(folderPath + @"\ZoneConditioning.csv"))
            {
                List<CSZoneConditioning> inZoneConditioning = readLibCSV<CSZoneConditioning>(folderPath + @"\ZoneConditioning.csv");
                newLib.AddRange(inZoneConditioning);
            }
            if (File.Exists(folderPath + @"\ZoneConstruction.csv"))
            {
                List<CSZoneConstruction> inZoneConstruction = readLibCSV<CSZoneConstruction>(folderPath + @"\ZoneConstruction.csv");
                newLib.AddRange(inZoneConstruction);
            }
            if (File.Exists(folderPath + @"\ZoneLoad.csv"))
            {
                List<CSZoneLoad> inZoneLoad = readLibCSV<CSZoneLoad>(folderPath + @"\ZoneLoad.csv");
                newLib.AddRange(inZoneLoad);
            }
            if (File.Exists(folderPath + @"\DomHotWater.csv"))
            {
                List<CSZoneHotWater> inDomHotWater = readLibCSV<CSZoneHotWater>(folderPath + @"\DomHotWater.csv");
                newLib.AddRange(inDomHotWater);
            }

            if (File.Exists(folderPath + @"\ZoneDefinition.csv"))
            {
                List<CSZoneDefinition> inZoneDefinition = readZoneDefCSV(folderPath + @"\ZoneDefinition.csv", newLib);
                newLib.AddRange(inZoneDefinition);
            }
            if (File.Exists(folderPath + @"\WindowSettings.csv"))
            {
                List<CSWindowDefinition> inWinSet = readLibCSV<CSWindowDefinition>(folderPath + @"\WindowSettings.csv");
                newLib.AddRange(inWinSet);
            }

            //var defaultLib = LibraryDefaults.GetDefaultLibrary();
            //defaultLib.Merge(lib);

            sp.Stop();
            Debug.WriteLine("Reading CSV Library: " +sp.ElapsedMilliseconds);


            return newLib;
        }




    }
}
