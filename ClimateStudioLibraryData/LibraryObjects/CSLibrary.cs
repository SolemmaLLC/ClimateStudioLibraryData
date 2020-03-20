using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CSEnergyLib.LibraryObjects
{
    /// <summary>
    /// Library that holds all materials, constructions and schedules during runtime.
    /// </summary>  
    [DataContract]
    [ProtoContract]
    public class CSLibrary
    {

        #region Add Low Level Objects

        public CSOpaqueConstruction Add(CSOpaqueConstruction obj)
        {
            if (obj == null) return null;
            if (OpaqueConstructions == null) OpaqueConstructions = new List<CSOpaqueConstruction>();

            foreach (var m in obj.Layers) this.Add(m.Material);

            if (!OpaqueConstructions.Any(i => i.Name == obj.Name))
            {
                OpaqueConstructions.Add(obj);
                return obj;
            }
            else
            {
                var oc = OpaqueConstructions.First(o => o.Name == obj.Name);

                if(!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                return oc;
            }
        }

        public CSGlazingConstruction Add(CSGlazingConstruction obj)
        {
            if (obj == null) return null;
            if (GlazingConstructions == null) GlazingConstructions = new List<CSGlazingConstruction>();


            foreach (var m in obj.Layers)
            {
                if (m is CSGlazingMaterial) { this.Add((CSGlazingMaterial)m); }
            }


            if (!GlazingConstructions.Any(i => i.Name == obj.Name))
            {
                GlazingConstructions.Add(obj);
                return obj;
            }
            else
            {
                var oc = GlazingConstructions.First(o => o.Name == obj.Name);

                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                return oc;
            }
        }
        public CSOpaqueMaterial Add(CSOpaqueMaterial obj)
        {
            if (obj == null) return null;
            if (OpaqueMaterials == null) OpaqueMaterials = new List<CSOpaqueMaterial>();
            if (!OpaqueMaterials.Any(i => i.Name == obj.Name))
            {
                OpaqueMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = OpaqueMaterials.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }

        public OpaqueMaterialNoMass Add(OpaqueMaterialNoMass obj)
        {
            if (obj == null) return null;
            if (NoMass == null) NoMass = new List<OpaqueMaterialNoMass>();
            if (!NoMass.Any(i => i.Name == obj.Name))
            {

                NoMass.Add(obj);

                return obj;
            }
            else
            {
                var oc = NoMass.First(o => o.Name == obj.Name);

                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                return oc;
            }
        }
        public OpaqueMaterialAirGap Add(OpaqueMaterialAirGap obj)
        {
            if (obj == null) return null;
            if (AirGap == null) AirGap = new List<OpaqueMaterialAirGap>();
            if (!AirGap.Any(i => i.Name == obj.Name))
            {

                AirGap.Add(obj);

                return obj;
            }
            else
            {
                var oc = AirGap.First(o => o.Name == obj.Name);

                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                return oc;
            }
        }


        public CSGlazingMaterial Add(CSGlazingMaterial obj)
        {
            if (obj == null) return null;
            if (GlazingMaterials == null) GlazingMaterials = new List<CSGlazingMaterial>();
            if (!GlazingMaterials.Any(i => i.Name == obj.Name))
            {
                GlazingMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = GlazingMaterials.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public CSGlazingConstructionSimple Add(CSGlazingConstructionSimple obj)
        {

            if (obj == null) return null;
            if (GlazingConstructionsSimple == null) GlazingConstructionsSimple = new List<CSGlazingConstructionSimple>();
            if (!GlazingConstructionsSimple.Any(i => i.Name == obj.Name))
            {
                GlazingConstructionsSimple.Add(obj);
                return obj;
            }
            else
            {
                var oc = GlazingConstructionsSimple.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;

            }
        }
        public CSGasMaterial Add(CSGasMaterial obj)
        {
            if (obj == null) return null;
            if (GasMaterials == null) GasMaterials = new List<CSGasMaterial>();
            if (!GasMaterials.Any(i => i.Name == obj.Name))
            {
                GasMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = GasMaterials.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public CSDaySchedule Add(CSDaySchedule obj)
        {
            if (obj == null) return null;
            if (DaySchedules == null) DaySchedules = new List<CSDaySchedule>();
            if (!DaySchedules.Any(i => i.Name == obj.Name))
            {
                DaySchedules.Add(obj);
                return obj;
            }
            else
            {
                var oc = DaySchedules.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }

        public CSYearSchedule Add(CSYearSchedule obj)
        {
            if (obj == null) return null;
            if (YearSchedules == null) YearSchedules = new List<CSYearSchedule>();
            if (!YearSchedules.Any(i => i.Name == obj.Name))
            {
                YearSchedules.Add(obj);

                HashSet<CSDaySchedule> days = new HashSet<CSDaySchedule>();
 
                foreach (var d in obj.WeekSchedules.SelectMany(x => x.Days))
                {
                    days.Add(d);
                }
 
                foreach (var d in days)
                {
                    this.Add(d);
                }
 

                return obj;
            }
            else
            {
                var oc = YearSchedules.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                HashSet<CSDaySchedule> days = new HashSet<CSDaySchedule>();

                foreach (var d in obj.WeekSchedules.SelectMany(x => x.Days))
                {
                    days.Add(d);
                }

                foreach (var d in days)
                {
                    this.Add(d);
                }

                return oc;
            }
        }
        public CSArraySchedule Add(CSArraySchedule obj)
        {
            if (obj == null) return null;
            if (ArraySchedules == null) ArraySchedules = new List<CSArraySchedule>();
            if (!ArraySchedules.Any(i => i.Name == obj.Name))
            {
                ArraySchedules.Add(obj);
                return obj;
            }
            else
            {
                var oc = ArraySchedules.First(o => o.Name == obj.Name);
                if (!oc.IsLocked) CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }








        public void AddRange(List<CSOpaqueConstruction> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }

        public void AddRange(List<CSGlazingConstruction> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }

        public void AddRange(List<CSOpaqueMaterial> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<OpaqueMaterialNoMass> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<OpaqueMaterialAirGap> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSGlazingMaterial> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSGlazingConstructionSimple> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSGasMaterial> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSDaySchedule> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSYearSchedule> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSArraySchedule> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneLoad> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneVentilation> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneHotWater> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneConstruction> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneConditioning> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSZoneDefinition> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }
        public void AddRange(List<CSWindowDefinition> objs)
        {
            foreach (var obj in objs) this.Add(obj);
        }


        #endregion

        #region Add High Level Objects

        public CSZoneLoad Add(CSZoneLoad obj)
        {
            if (obj == null) return null;
            if (ZoneLoads == null) ZoneLoads = new List<CSZoneLoad>();
            if (!ZoneLoads.Any(i => i.Name == obj.Name))
            {
                ZoneLoads.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneLoads.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public CSZoneVentilation Add(CSZoneVentilation obj)
        {
            if (obj == null) return null;
            if (ZoneVentilations == null) ZoneVentilations = new List<CSZoneVentilation>();
            if (!ZoneVentilations.Any(i => i.Name == obj.Name))
            {
                ZoneVentilations.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneVentilations.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public CSZoneConstruction Add(CSZoneConstruction obj)
        {
            if (obj == null) return null;
            if (ZoneConstructions == null) ZoneConstructions = new List<CSZoneConstruction>();
            if (!ZoneConstructions.Any(i => i.Name == obj.Name))
            {
                ZoneConstructions.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneConstructions.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public CSZoneConditioning Add(CSZoneConditioning obj)
        {
            if (obj == null) return null;
            if (ZoneConditionings == null) ZoneConditionings = new List<CSZoneConditioning>();
            if (!ZoneConditionings.Any(i => i.Name == obj.Name))
            {
                ZoneConditionings.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneConditionings.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public CSZoneHotWater Add(CSZoneHotWater obj)
        {
            if (obj == null) return null;
            if (DomHotWaters == null) DomHotWaters = new List<CSZoneHotWater>();
            if (!DomHotWaters.Any(i => i.Name == obj.Name))
            {
                DomHotWaters.Add(obj);
                return obj;
            }
            else
            {
                var oc = DomHotWaters.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public CSZoneDefinition Add(CSZoneDefinition obj)
        {
            if (obj == null) return null;
            if (ZoneDefinitions == null) ZoneDefinitions = new List<CSZoneDefinition>();
            if (!ZoneDefinitions.Any(i => i.Name == obj.Name))
            {
                ZoneDefinitions.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneDefinitions.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }

        public CSWindowDefinition Add(CSWindowDefinition obj)
        {
            if (obj == null) return null;

            if (!WindowSettings.Any(i => i.Name == obj.Name))
            {
                WindowSettings.Add(obj);
                return obj;
            }
            else
            {
                var oc = WindowSettings.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public FloorDefinition Add(FloorDefinition obj)
        {
            if (obj == null) return null;
            if (FloorDefinitions == null) FloorDefinitions = new List<FloorDefinition>();
            if (!FloorDefinitions.Any(i => i.Name == obj.Name))
            {
                FloorDefinitions.Add(obj);
                return obj;
            }
            else
            {
                var oc = FloorDefinitions.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }

        public BuildingDefinition Add(BuildingDefinition obj)
        {
            if (obj == null) return null;
            if (FloorDefinitions == null) BuildingDefinitions = new List<BuildingDefinition>();
            if (!BuildingDefinitions.Any(i => i.Name == obj.Name))
            {
                BuildingDefinitions.Add(obj);
                return obj;
            }
            else
            {
                var oc = BuildingDefinitions.First(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        #endregion


        #region Delete Objects

        public void Delete(CSZoneDefinition obj)
        {
            if (ZoneDefinitions.Contains(obj))
            {
                ZoneDefinitions.Remove(obj);
            }
        }


        #endregion


        [ProtoIgnore]
        public string Name = ""; // set at runtime

        [ProtoIgnore]
        public bool IsLocked = false; // set at runtime

        [ProtoIgnore]
        public bool IsDefault = false; // set at runtime


        [DataMember(Order = 1)]
        [ProtoMember(1)]
        public string Version;


        [DataMember(Order = 1)]
        [ProtoMember(2)]
        public DateTime TimeStamp;


        //low level objects
        [DataMember(Order = 1)]
        [ProtoMember(3, AsReference = true)]
        public List<CSOpaqueMaterial> OpaqueMaterials;


        [DataMember(Order = 1)]
        [ProtoMember(4, AsReference = true)]
        public List<OpaqueMaterialNoMass> NoMass;


        [DataMember(Order = 1)]
        [ProtoMember(5, AsReference = true)]
        public List<OpaqueMaterialAirGap> AirGap;


        [DataMember(Order = 1)]
        [ProtoMember(6, AsReference = true)]
        public List<CSGlazingMaterial> GlazingMaterials;


        [DataMember(Order = 1)]
        [ProtoMember(7, AsReference = true)]
        public List<CSGasMaterial> GasMaterials;



        [DataMember(Order = 1)]
        [ProtoMember(100, AsReference = true)]
        public List<CSGlazingConstructionSimple> GlazingConstructionsSimple;


        [DataMember(Order = 2)]
        [ProtoMember(101, AsReference = true)]
        public List<CSOpaqueConstruction> OpaqueConstructions;


        [DataMember(Order = 2)]
        [ProtoMember(102, AsReference = true)]
        public List<CSGlazingConstruction> GlazingConstructions;


        [DataMember(Order = 10)]
        [ProtoMember(103, AsReference = true)]
        public List<CSDaySchedule> DaySchedules;


        [DataMember(Order = 12)]
        [ProtoMember(104, AsReference = true)]
        public List<CSYearSchedule> YearSchedules;


        [DataMember(Order = 13)]
        [ProtoMember(105, AsReference = true)]
        public List<CSArraySchedule> ArraySchedules;



        //zone definitions
        [DataMember(Order = 20)]
        [ProtoMember(1000, AsReference = true)]
        public List<CSZoneLoad> ZoneLoads;


        [DataMember(Order = 20)]
        [ProtoMember(1001, AsReference = true)]
        public List<CSZoneVentilation> ZoneVentilations;


        [DataMember(Order = 20)]
        [ProtoMember(1002, AsReference = true)]
        public List<CSZoneConstruction> ZoneConstructions;


        [DataMember(Order = 20)]
        [ProtoMember(1003, AsReference = true)]
        public List<CSZoneConditioning> ZoneConditionings;


        [DataMember(Order = 20)]
        [ProtoMember(1004, AsReference = true)]
        public List<CSZoneHotWater> DomHotWaters;



        [DataMember(Order = 30)]
        [ProtoMember(1005, AsReference = true)]
        public List<CSZoneDefinition> ZoneDefinitions;


        [DataMember(Order = 30)]
        [ProtoMember(1006, AsReference = true)]
        public List<CSWindowDefinition> WindowSettings;


        [DataMember(Order = 30)]
        [ProtoMember(1007, AsReference = true)]
        public List<FloorDefinition> FloorDefinitions;



        [DataMember(Order = 40)]
        [ProtoMember(1008, AsReference = true)]
        public List<BuildingDefinition> BuildingDefinitions;


 

        public CSLibrary()
        {
            Version = AssembleyInfo.AssemblyVersion;
            TimeStamp = DateTime.Now;
            OpaqueMaterials = new List<CSOpaqueMaterial>();
            AirGap = new List<OpaqueMaterialAirGap>();
            NoMass = new List<OpaqueMaterialNoMass>();
            GlazingMaterials = new List<CSGlazingMaterial>();
            GasMaterials = new List<CSGasMaterial>();
            OpaqueConstructions = new List<CSOpaqueConstruction>();
            GlazingConstructions = new List<CSGlazingConstruction>();
            GlazingConstructionsSimple = new List<CSGlazingConstructionSimple>();
            DaySchedules = new List<CSDaySchedule>();
            YearSchedules = new List<CSYearSchedule>();
            ArraySchedules = new List<CSArraySchedule>();
            ZoneLoads = new List<CSZoneLoad>();
            ZoneVentilations = new List<CSZoneVentilation>();
            ZoneConstructions = new List<CSZoneConstruction>();
            ZoneConditionings = new List<CSZoneConditioning>();
            DomHotWaters = new List<CSZoneHotWater>();
            ZoneDefinitions = new List<CSZoneDefinition>();
            WindowSettings = new List<CSWindowDefinition>();

            FloorDefinitions = new List<FloorDefinition>();
            BuildingDefinitions = new List<BuildingDefinition>();
        }




        public string Import(string path)
        {
            string s = "";
            try
            {

                if (File.Exists(path))
                {
                    CSLibrary ImportedLibrary = null;
                    string errPath = Path.GetDirectoryName(path);
                    string json = File.ReadAllText(path);
                    ImportedLibrary = CSLibrary.fromJSON(json);


                    if (ImportedLibrary.Correct(out s))
                    {
                        File.WriteAllText(errPath + "/ImportErrors.txt", s);
                    }

                    Import(ImportedLibrary);

                    Debug.WriteLine("Library loaded from " + path);
                }

            }
            catch (Exception ex) { Debug.WriteLine("Library import error " + ex.Message); }

            return s;
        }

        public string Merge(string path)
        {
            string s = "";
            try
            {

                if (File.Exists(path))
                {
                    CSLibrary ImportedLibrary = null;
                    string errPath = Path.GetDirectoryName(path);
                    string json = File.ReadAllText(path);
                    ImportedLibrary = CSLibrary.fromJSON(json);


                    if (ImportedLibrary.Correct(out s))
                    {
                        File.WriteAllText(errPath + "/ImportErrors.txt", s);
                    }

                    Merge(ImportedLibrary);

                    Debug.WriteLine("Library loaded and merged from " + path);
                }

            }
            catch (Exception ex) { Debug.WriteLine("Library import error " + ex.Message); }

            return s;
        }

        public void Import(CSLibrary ImportedLibrary)
        {
            TimeStamp = ImportedLibrary.TimeStamp;
            Version = ImportedLibrary.Version;

            this.Clear();

            this.Merge(LibraryDefaults.GetMinimumRequiredBaseLibrary()); // place objects in lib in first place.

            this.Merge(ImportedLibrary);

            //this.Merge(LibraryDefaults.GetMinimumRequiredBaseLibrary()); // override with defaults in case imported lib contained data with identical ids.

        }

        public void Merge(CSLibrary ImportedLibrary)
        {
            TimeStamp = ImportedLibrary.TimeStamp;
            Version = ImportedLibrary.Version;

            foreach (var o in ImportedLibrary.OpaqueMaterials) this.Add(o);
            foreach (var o in ImportedLibrary.GlazingMaterials) this.Add(o);
            foreach (var o in ImportedLibrary.GasMaterials) this.Add(o);
            foreach (var o in ImportedLibrary.OpaqueConstructions) this.Add(o);
            foreach (var o in ImportedLibrary.GlazingConstructions) this.Add(o);
            foreach (var o in ImportedLibrary.GlazingConstructionsSimple) this.Add(o);
            foreach (var o in ImportedLibrary.DaySchedules) this.Add(o); 
            foreach (var o in ImportedLibrary.YearSchedules) this.Add(o);
            foreach (var o in ImportedLibrary.ArraySchedules) this.Add(o);


            foreach (var o in ImportedLibrary.ZoneLoads) this.Add(o);
            foreach (var o in ImportedLibrary.ZoneVentilations) this.Add(o);
            foreach (var o in ImportedLibrary.ZoneConstructions) this.Add(o);
            foreach (var o in ImportedLibrary.ZoneConditionings) this.Add(o);
            foreach (var o in ImportedLibrary.DomHotWaters) this.Add(o);


            foreach (var o in ImportedLibrary.ZoneDefinitions) this.Add(o);

            foreach (var o in ImportedLibrary.WindowSettings) this.Add(o);

            foreach (var o in ImportedLibrary.FloorDefinitions) this.Add(o);

            foreach (var o in ImportedLibrary.BuildingDefinitions) this.Add(o);

        }

        public void Append(CSLibrary ImportedLibrary)
        {
            //TimeStamp = ImportedLibrary.TimeStamp;
            //Version = ImportedLibrary.Version;
            foreach (var o in ImportedLibrary.OpaqueMaterials) this.OpaqueMaterials.Add(o);
            foreach (var o in ImportedLibrary.GlazingMaterials) this.GlazingMaterials.Add(o);
            foreach (var o in ImportedLibrary.GasMaterials) this.GasMaterials.Add(o);
            foreach (var o in ImportedLibrary.OpaqueConstructions) this.OpaqueConstructions.Add(o);
            foreach (var o in ImportedLibrary.GlazingConstructions) this.GlazingConstructions.Add(o);
            foreach (var o in ImportedLibrary.GlazingConstructionsSimple) this.GlazingConstructionsSimple.Add(o);
            foreach (var o in ImportedLibrary.DaySchedules) this.DaySchedules.Add(o);
            foreach (var o in ImportedLibrary.YearSchedules) this.YearSchedules.Add(o);
            foreach (var o in ImportedLibrary.ArraySchedules) this.ArraySchedules.Add(o);
            foreach (var o in ImportedLibrary.ZoneLoads) this.ZoneLoads.Add(o);
            foreach (var o in ImportedLibrary.ZoneVentilations) this.ZoneVentilations.Add(o);
            foreach (var o in ImportedLibrary.ZoneConstructions) this.ZoneConstructions.Add(o);
            foreach (var o in ImportedLibrary.ZoneConditionings) this.ZoneConditionings.Add(o);
            foreach (var o in ImportedLibrary.DomHotWaters) this.DomHotWaters.Add(o);
            foreach (var o in ImportedLibrary.ZoneDefinitions) this.ZoneDefinitions.Add(o);
            foreach (var o in ImportedLibrary.WindowSettings) this.WindowSettings.Add(o);
            foreach (var o in ImportedLibrary.FloorDefinitions) this.FloorDefinitions.Add(o);
            foreach (var o in ImportedLibrary.BuildingDefinitions) this.BuildingDefinitions.Add(o);
        }

        public void Clear()
        {

            try
            {
                TimeStamp = DateTime.Now;
                OpaqueMaterials.Clear();
                GlazingMaterials.Clear();
                GasMaterials.Clear();
                OpaqueConstructions.Clear();
                GlazingConstructions.Clear();
                GlazingConstructionsSimple.Clear();
                DaySchedules.Clear();
                YearSchedules.Clear();
                ArraySchedules.Clear();

                ZoneLoads.Clear();
                ZoneVentilations.Clear();
                ZoneConstructions.Clear();
                ZoneConditionings.Clear();
                DomHotWaters.Clear();
                ZoneDefinitions.Clear();

                WindowSettings.Clear();
                FloorDefinitions.Clear();
                BuildingDefinitions.Clear();
            }
            catch { }
        }

        public void Reset()
        {
            this.Clear();
            this.Merge(LibraryDefaults.GetDefaultLibrary());
        }

        public T getElementByName<T>(string name)
        {
            // materials and constructions

            try
            {
                if (typeof(T) == typeof(CSOpaqueConstruction))
                {
                    return (T)Convert.ChangeType(OpaqueConstructions.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSGlazingConstruction))
                {
                    return (T)Convert.ChangeType(GlazingConstructions.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSOpaqueMaterial))
                {
                    return (T)Convert.ChangeType(OpaqueMaterials.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSGlazingMaterial))
                {
                    return (T)Convert.ChangeType(GlazingMaterials.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSGasMaterial))
                {
                    return (T)Convert.ChangeType(GasMaterials.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSGlazingConstructionSimple))
                {
                    return (T)Convert.ChangeType(GlazingConstructionsSimple.First(o => o.Name == name), typeof(T));
                }

                // schedules

                else if (typeof(T) == typeof(CSDaySchedule))
                {
                    return (T)Convert.ChangeType(DaySchedules.First(o => o.Name == name), typeof(T));
                }
                //else if (typeof(T) == typeof(WeekSchedule))
                //{
                //    return (T)Convert.ChangeType(WeekSchedules.First(o => o.Name == name), typeof(T));
                //}
                else if (typeof(T) == typeof(CSYearSchedule))
                {
                    return (T)Convert.ChangeType(YearSchedules.First(o => o.Name == name), typeof(T));
                }

                else if (typeof(T) == typeof(CSArraySchedule))
                {
                    return (T)Convert.ChangeType(ArraySchedules.First(o => o.Name == name), typeof(T));
                }

                // zone def

                else if (typeof(T) == typeof(CSZoneLoad))
                {
                    return (T)Convert.ChangeType(ZoneLoads.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSZoneVentilation))
                {
                    return (T)Convert.ChangeType(ZoneVentilations.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSZoneConstruction))
                {
                    return (T)Convert.ChangeType(ZoneConstructions.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSZoneConditioning))
                {
                    return (T)Convert.ChangeType(ZoneConditionings.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSZoneHotWater))
                {
                    return (T)Convert.ChangeType(DomHotWaters.First(o => o.Name == name), typeof(T));
                }
                else if (typeof(T) == typeof(CSZoneDefinition))
                {
                    return (T)Convert.ChangeType(ZoneDefinitions.First(o => o.Name == name), typeof(T));
                }

                else if (typeof(T) == typeof(CSWindowDefinition))
                {
                    return (T)Convert.ChangeType(WindowSettings.First(o => o.Name == name), typeof(T));
                }

                else if (typeof(T) == typeof(FloorDefinition))
                {
                    return (T)Convert.ChangeType(FloorDefinitions.First(o => o.Name == name), typeof(T));
                }

                // dont know what this is???

                else return (T)Convert.ChangeType(null, typeof(T));

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not find " + name + ": " + ex.Message);
                return default(T);
            }
        }

        public CSLibrary getFromSpecificLibrary(string libraryName)
        {

            CSLibrary newLib = new CSLibrary();

            newLib.TimeStamp = this.TimeStamp;
            newLib.Version = this.Version;

            foreach (var o in this.OpaqueMaterials) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.GlazingMaterials) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.GasMaterials) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.GlazingConstructionsSimple) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.DaySchedules) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.YearSchedules) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ArraySchedules) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ZoneLoads) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ZoneVentilations) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ZoneConstructions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ZoneConditionings) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.DomHotWaters) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.ZoneDefinitions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.WindowSettings) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.FloorDefinitions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            foreach (var o in this.BuildingDefinitions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };

            //foreach (var o in this.YearSchedules) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            //foreach (var o in this.OpaqueConstructions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            //foreach (var o in this.GlazingConstructions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };
            //foreach (var o in this.ZoneDefinitions) { if (o.LibraryName == libraryName) { newLib.Add(o); } };

            foreach (var o in this.YearSchedules) { if (o.LibraryName == libraryName) { 
                    newLib.Add(o);
                    foreach (var d in o.WeekSchedules.SelectMany(x => x.Days)) {
                        this.Add(d);
                    }
                } };
            foreach (var o in this.OpaqueConstructions) { if (o.LibraryName == libraryName) { 
                    newLib.Add(o);
                    foreach (var m in o.Layers.Select(x => x.Material))
                    {
                        this.Add(m);
                    }
                } };
            foreach (var o in this.GlazingConstructions) { if (o.LibraryName == libraryName) { 
                    newLib.Add(o);
                    foreach (var m in o.Layers)
                    {
                        if (m is CSGlazingMaterial)
                        {
                            this.Add(m as CSGlazingMaterial);
                        }
                        else
                        {
                            this.Add(m as CSGasMaterial);
                        }
                    }
                } };
            foreach (var o in this.ZoneDefinitions) { 
                if (o.LibraryName == libraryName) {  
                   
                    o.Loads.Name = o.Name;
                    newLib.Add(o.Loads);
                    o.Conditioning.Name = o.Name;
                    newLib.Add(o.Conditioning);
                    o.Ventilation.Name = o.Name;
                    newLib.Add(o.Ventilation);
                    o.DomHotWater.Name = o.Name;
                    newLib.Add(o.DomHotWater);
                    o.Materials.Name = o.Name;
                    newLib.Add(o.Materials);

                    newLib.Add(o);
                } };

            return newLib;
        }
 
        public void applyLibName(string libraryName)
        {
            foreach (var o in this.OpaqueMaterials) { o.LibraryName = libraryName; };
            foreach (var o in this.GlazingMaterials) { o.LibraryName = libraryName; };
            foreach (var o in this.GasMaterials) { o.LibraryName = libraryName; };
            foreach (var o in this.OpaqueConstructions) { o.LibraryName = libraryName; };
            foreach (var o in this.GlazingConstructions) { o.LibraryName = libraryName; };
            foreach (var o in this.GlazingConstructionsSimple) { o.LibraryName = libraryName; };
            foreach (var o in this.DaySchedules) { o.LibraryName = libraryName; };
            foreach (var o in this.YearSchedules) { o.LibraryName = libraryName; };
            foreach (var o in this.ArraySchedules) { o.LibraryName = libraryName; };
            foreach (var o in this.ZoneLoads) { o.LibraryName = libraryName; };
            foreach (var o in this.ZoneVentilations) { o.LibraryName = libraryName; };
            foreach (var o in this.ZoneConstructions) { o.LibraryName = libraryName; };
            foreach (var o in this.ZoneConditionings) { o.LibraryName = libraryName; };
            foreach (var o in this.DomHotWaters) { o.LibraryName = libraryName; };
            foreach (var o in this.ZoneDefinitions) { o.LibraryName = libraryName; };
            foreach (var o in this.WindowSettings) { o.LibraryName = libraryName; };
            foreach (var o in this.FloorDefinitions) { o.LibraryName = libraryName; };
            foreach (var o in this.BuildingDefinitions) { o.LibraryName = libraryName; };
        }
        public void applyLock(bool isLocked)
        {
            foreach (var o in this.OpaqueMaterials) { o.IsLocked = isLocked; };
            foreach (var o in this.GlazingMaterials) { o.IsLocked = isLocked; };
            foreach (var o in this.GasMaterials) { o.IsLocked = isLocked; };
            foreach (var o in this.OpaqueConstructions) { o.IsLocked = isLocked; };
            foreach (var o in this.GlazingConstructions) { o.IsLocked = isLocked; };
            foreach (var o in this.GlazingConstructionsSimple) { o.IsLocked = isLocked; };
            foreach (var o in this.DaySchedules) { o.IsLocked = isLocked; };
            foreach (var o in this.YearSchedules) { o.IsLocked = isLocked; };
            foreach (var o in this.ArraySchedules) { o.IsLocked = isLocked; };
            foreach (var o in this.ZoneLoads) { o.IsLocked = isLocked; };
            foreach (var o in this.ZoneVentilations) { o.IsLocked = isLocked; };
            foreach (var o in this.ZoneConstructions) { o.IsLocked = isLocked; };
            foreach (var o in this.ZoneConditionings) { o.IsLocked = isLocked; };
            foreach (var o in this.DomHotWaters) { o.IsLocked = isLocked; };
            foreach (var o in this.ZoneDefinitions) { o.IsLocked = isLocked; };
            foreach (var o in this.WindowSettings) { o.IsLocked = isLocked; };
            foreach (var o in this.FloorDefinitions) { o.IsLocked = isLocked; };
            foreach (var o in this.BuildingDefinitions) { o.IsLocked = isLocked; };
        }
        public void applyIsDefault(bool isDefault)
        {
            foreach (var o in this.OpaqueMaterials) { o.IsDefault  = isDefault; };
            foreach (var o in this.GlazingMaterials) { o.IsDefault = isDefault; };
            foreach (var o in this.GasMaterials) { o.IsDefault = isDefault; };
            foreach (var o in this.OpaqueConstructions) { o.IsDefault = isDefault; };
            foreach (var o in this.GlazingConstructions) { o.IsDefault = isDefault; };
            foreach (var o in this.GlazingConstructionsSimple) { o.IsDefault = isDefault; };
            foreach (var o in this.DaySchedules) { o.IsDefault = isDefault; };
            foreach (var o in this.YearSchedules) { o.IsDefault = isDefault; };
            foreach (var o in this.ArraySchedules) { o.IsDefault = isDefault; };
            foreach (var o in this.ZoneLoads) { o.IsDefault = isDefault; };
            foreach (var o in this.ZoneVentilations) { o.IsDefault = isDefault; };
            foreach (var o in this.ZoneConstructions) { o.IsDefault = isDefault; };
            foreach (var o in this.ZoneConditionings) { o.IsDefault = isDefault; };
            foreach (var o in this.DomHotWaters) { o.IsDefault = isDefault; };
            foreach (var o in this.ZoneDefinitions) { o.IsDefault = isDefault; };
            foreach (var o in this.WindowSettings) { o.IsDefault = isDefault; };
            foreach (var o in this.FloorDefinitions) { o.IsDefault = isDefault; };
            foreach (var o in this.BuildingDefinitions) { o.IsDefault = isDefault; };
        }

        public List<LibraryComponent> getUserObjects()
        {

            List<LibraryComponent> newLib = new List<LibraryComponent>();

            foreach (var o in this.OpaqueMaterials) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.GlazingMaterials) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.GasMaterials) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.OpaqueConstructions) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.GlazingConstructions) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.GlazingConstructionsSimple) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.DaySchedules) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.YearSchedules) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ArraySchedules) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ZoneLoads) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ZoneVentilations) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ZoneConstructions) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ZoneConditionings) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.DomHotWaters) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.ZoneDefinitions) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.WindowSettings) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.FloorDefinitions) { if (!o.IsDefault) { newLib.Add(o); } };
            foreach (var o in this.BuildingDefinitions) { if (!o.IsDefault) { newLib.Add(o); } };

            return newLib;
        }

        public List<LibraryComponent> getAllObjects()
        {

            List<LibraryComponent> newLib = new List<LibraryComponent>();

            foreach (var o in this.OpaqueMaterials) {  newLib.Add(o);  };
            foreach (var o in this.GlazingMaterials) {  newLib.Add(o);  };
            foreach (var o in this.GasMaterials) {  newLib.Add(o);  };
            foreach (var o in this.OpaqueConstructions) {  newLib.Add(o);  };
            foreach (var o in this.GlazingConstructions) {  newLib.Add(o);  };
            foreach (var o in this.GlazingConstructionsSimple) {  newLib.Add(o);  };
            foreach (var o in this.DaySchedules) {  newLib.Add(o);  };
            foreach (var o in this.YearSchedules) {  newLib.Add(o);  };
            foreach (var o in this.ArraySchedules) {  newLib.Add(o);  };
            foreach (var o in this.ZoneLoads) {  newLib.Add(o);  };
            foreach (var o in this.ZoneVentilations) {  newLib.Add(o);  };
            foreach (var o in this.ZoneConstructions) {  newLib.Add(o);  };
            foreach (var o in this.ZoneConditionings) {  newLib.Add(o);  };
            foreach (var o in this.DomHotWaters) {  newLib.Add(o);  };
            foreach (var o in this.ZoneDefinitions) {  newLib.Add(o);  };
            foreach (var o in this.WindowSettings) {  newLib.Add(o);  };
            foreach (var o in this.FloorDefinitions) {  newLib.Add(o);  };
            foreach (var o in this.BuildingDefinitions) {  newLib.Add(o);  };

            return newLib;
        }

        public CSLibrary getUnLockedLib()
        {

            CSLibrary newLib = new CSLibrary();

            newLib.TimeStamp = this.TimeStamp;
            newLib.Version = this.Version;

            foreach (var o in this.OpaqueMaterials) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.GlazingMaterials) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.GasMaterials) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.OpaqueConstructions) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.GlazingConstructions) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.GlazingConstructionsSimple) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.DaySchedules) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.YearSchedules) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ArraySchedules) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ZoneLoads) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ZoneVentilations) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ZoneConstructions) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ZoneConditionings) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.DomHotWaters) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.ZoneDefinitions) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.WindowSettings) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.FloorDefinitions) { if (!o.IsLocked) { newLib.Add(o); } };
            foreach (var o in this.BuildingDefinitions) { if (!o.IsLocked) { newLib.Add(o); } };

            return newLib;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("OpaqueMaterial [" + OpaqueMaterials.Count + "]");
            sb.AppendLine("AirGap and NoMass [" + AirGap.Count + NoMass.Count + "]");
            sb.AppendLine("GlazingMaterial [" + GlazingMaterials.Count + "]");
            sb.AppendLine("GasMaterial [" + GasMaterials.Count + "]");
            sb.AppendLine("GlazingConstructionsSimple [" + GlazingConstructionsSimple.Count + "]");
            sb.AppendLine("OpaqueConstruction [" + OpaqueConstructions.Count + "]");
            sb.AppendLine("GlazingConstruction [" + GlazingConstructions.Count + "]");
            sb.AppendLine("DaySchedule [" + DaySchedules.Count + "]");
            sb.AppendLine("YearSchedule [" + YearSchedules.Count + "]");
            sb.AppendLine("ArraySchedule [" + ArraySchedules.Count + "]");
            sb.AppendLine("ZoneLoad [" + ZoneLoads.Count + "]");
            sb.AppendLine("ZoneVentilation [" + ZoneVentilations.Count + "]");
            sb.AppendLine("ZoneConstruction [" + ZoneConstructions.Count + "]");
            sb.AppendLine("ZoneConditioning [" + ZoneConditionings.Count + "]");
            sb.AppendLine("DomHotWater [" + DomHotWaters.Count + "]");
            sb.AppendLine("ZoneDefinition [" + ZoneDefinitions.Count + "]");
            sb.AppendLine("WindowSetting [" + WindowSettings.Count + "]");
            sb.AppendLine("FloorDefinitions [" + FloorDefinitions.Count + "]");
            sb.AppendLine("BuildingDefinition [" + BuildingDefinitions.Count + "]");


            return sb.ToString();

            // return base.ToString();
        }

        public static CSLibrary HardCopy(CSLibrary lib)
        {
            //string s = lib.toJSON();
            //return CSLibrary.fromJSON(s);
            string s = lib.buffMe();
            return CSLibrary.unBuffMe(s);
        }


        #region CheckForInvalidObjs


        private void reportInvalidOpaqueMaterials(ref string errorReport)
        {
            //Dictionary<string, string> invalidNames = new Dictionary<string, string>();
            try
            {
                foreach (CSOpaqueMaterial op in this.OpaqueMaterials)
                {

                    // find strange names
                    string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(op.Name);
                    if (op.Name != cleanName)
                    {
                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Material " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidOpaqueMaterials failed: " + ex.Message);// + " " + ex.InnerException.Message); }

            }
        }
        private void reportInvalidGlazingMaterials(ref string errorReport)
        {

            try
            {
                foreach (CSGlazingMaterial om in this.GlazingMaterials)
                {

                    // find strange names
                    string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(om.Name);
                    if (om.Name != cleanName)
                    {
                        errorReport += om.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        om.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (om.Correct())
                    {
                        errorReport += "Material " + om.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidGlazingMaterials failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }
        }
        private void reportInvalidOCons(ref string errorReport)
        {


            foreach (CSOpaqueConstruction op in this.OpaqueConstructions)
            {
                string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(op.Name);
                if (cleanName != op.Name)
                {

                    //op.Name = Formating.RemoveSpecialCharactersNotStrict(op.Name);
                    errorReport += "Construction  " + op.Name + " name contained invalid characters and has been auto corrected to " + cleanName + "\r\n";
                    op.Name = cleanName;
                }

                foreach (var ol in op.Layers)
                {

                    ol.Material.Name = CSFormatting.RemoveSpecialCharactersNotStrict(ol.Material.Name);// fix as in materials
                    if (ol.Correct())
                    {
                        errorReport += "Layer in  " + op.Name + " contained invalid thickness \r\n";
                    }
                }
            }
        }
        private void reportInvalidGCons(ref string errorReport)
        {


            foreach (CSGlazingConstruction op in this.GlazingConstructions)
            {
                string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(op.Name);
                if (cleanName != op.Name)
                {

                    //op.Name = Formating.RemoveSpecialCharactersNotStrict(op.Name);
                    errorReport += "Construction  " + op.Name + " name contained invalid characters and has been auto corrected to " + cleanName + "\r\n";
                    op.Name = cleanName;
                }

                foreach (var ol in op.Layers)
                {

                    ol.Name = CSFormatting.RemoveSpecialCharactersNotStrict(ol.Name);// fix as in materials
                    //if (ol.Correct())
                    //{
                    //    errorReport += "Layer in  " + op.Name + " contained invalid thickness \r\n";
                    //}
                }
            }
        }
        private void reportInvalidDaySchedules(ref string errorReport)
        {
            Dictionary<string, string> invalidNames = new Dictionary<string, string>();
            try
            {
                foreach (CSDaySchedule op in this.DaySchedules)
                {


                    // find strange names
                    string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(op.Name);
                    if (op.Name != cleanName)
                    {

                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Schedule " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidDaySchedules failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }

        }
     

        private void reportInvalidYearSchedules(ref string errorReport)
        {

            try
            {
                foreach (CSYearSchedule op in this.YearSchedules)
                {

                    // find strange names
                    string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(op.Name);
                    if (op.Name != cleanName)
                    {

                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Schedule " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidYearSchedules failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }

        }

        public bool Correct(out string err)
        {

            string a = "";


            reportInvalidOpaqueMaterials(ref a);
            reportInvalidGlazingMaterials(ref a);
            reportInvalidOCons(ref a);
            reportInvalidGCons(ref a);
            reportInvalidDaySchedules(ref a);
            //reportInvalidWeekSchedules(ref a);
            reportInvalidYearSchedules(ref a);



            err =
                // "======= Error report =======\r\n\r\n" +
                a;
            //+ "\r\n=======  Report End  =======";

            if (a == "") return false;
            else return true;

        }

        #endregion


        public static CSLibrary fromJSON(string json)
        {
            return Serialization.Deserialize<CSLibrary>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSLibrary>(this);
        }
        public string buffMe()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, this);
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
        public static CSLibrary unBuffMe( string txt)
        {
            byte[] arr = Convert.FromBase64String(txt);
            using (MemoryStream ms = new MemoryStream(arr))
                return ProtoBuf.Serializer.Deserialize<CSLibrary>(ms);
        }

        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' 
        /// of the first. 
        /// This any matching properties are written to the target.
        /// 
        /// The object copy is a shallow copy only. Any nested types will be copied as 
        /// whole values rather than individual property assignments (ie. via assignment)
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }





















    }


}


