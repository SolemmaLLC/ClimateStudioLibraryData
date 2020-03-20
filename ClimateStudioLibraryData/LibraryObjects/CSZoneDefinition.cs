using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract]
    [ProtoContract]

    public class CSZoneDefinition : LibraryComponent
    {


        [DataMember, DefaultValue(1.0)]
        [ProtoMember(1)]
        public double ZoneMultiplier { get; set; } = 1.0;



        [DataMember, DefaultValue(45.0), Units("deg")]
        [ProtoMember(2)]
        public double RoofTiltAngle { get; set; } = 45.0;



        [DataMember, DefaultValue(135.0), Units("deg")]
        [ProtoMember(3)]
        public double FloorTiltAngle { get; set; } = 135.0;







        [DataMember, DefaultValue(0.500)]
        [Units("Kg/kWh")]
        [ProtoMember(50)]
        public double ElectricityCO2 { get; set; } = 0.500;

        [DataMember, DefaultValue(0.2)]
        [Units("$/kWh")]
        [ProtoMember(51)]
        public double ElectricityCost { get; set; } = 0.2;

        [DataMember, DefaultValue(0.500)]
        [Units("Kg/kWh")]
        [ProtoMember(52)]
        public double HotWaterCO2 { get; set; } = 0.500;

        [DataMember, DefaultValue(0.2)]
        [Units("$/kWh")]
        [ProtoMember(53)]
        public double HotWaterCost { get; set; } = 0.2;

        [DataMember, DefaultValue(0.500)]
        [Units("Kg/kWh")]
        [ProtoMember(54)]
        public double CoolingCO2 { get; set; } = 0.500;

        [DataMember, DefaultValue(0.500)]
        [Units("Kg/kWh")]
        [ProtoMember(55)]
        public double HeatingCO2 { get; set; } = 0.500;

        [DataMember, DefaultValue(0.2)]
        [Units("$/kWh")]
        [ProtoMember(56)]
        public double CoolingCost { get; set; } = 0.2;

        [DataMember, DefaultValue(0.2)]
        [Units("$/kWh")]
        [ProtoMember(57)]
        public double HeatingCost { get; set; } = 0.2;






        //MAT
        //-----
        [DataMember]
        [ProtoMember(100, AsReference = true)]

        public CSZoneConstruction Materials { get; set; } = new CSZoneConstruction();

        //LOADS
        //-----
        [DataMember]
        [ProtoMember(101, AsReference = true)]

        public CSZoneLoad Loads { get; set; } = new CSZoneLoad();

        //IdealLoadsAirSystem
        //-------------------
        [DataMember]
        [ProtoMember(102, AsReference = true)]

        public CSZoneConditioning Conditioning { get; set; } = new CSZoneConditioning();

        //DOM HOT WAT
        [DataMember]
        [ProtoMember(103, AsReference = true)]

        public CSZoneHotWater DomHotWater { get; set; } = new CSZoneHotWater();

        //AIRFLOW / VENT
        //--------------
        [DataMember]
        [ProtoMember(104, AsReference = true)]

        public CSZoneVentilation Ventilation { get; set; } = new CSZoneVentilation();





        public CSZoneDefinition()
        {
        }

        public static CSZoneDefinition HardCopy(CSZoneDefinition zsc)
        {
            //string s = zsc.toJSON();
            //return CSZoneDefinition.fromJSON(s);
            string s = zsc.buffMe();
            return CSZoneDefinition.unBuffMe(s);
        }

        public void Rename(string newName) {

            this.Name = newName;
            this.Loads.Name = newName;
            this.Conditioning.Name = newName;
            this.Ventilation.Name = newName;
            this.DomHotWater.Name = newName;
            this.Materials.Name = newName;
 
        }

        public   string GetInfo() {  
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Loads.Program);
            sb.Append(" | ");
            sb.Append(this.Loads.ZoneUseType.ToString());
            //sb.Append(" | Template: ");
            //sb.Append(this.Name);


            //sb.Append(" People:");
            //sb.Append(Math.Round(this.Loads.PeopleDensity, 2));
            //sb.Append("[p/m2] Facade: ");
            //sb.Append(this.Materials.FacadeConstruction) ;


            return sb.ToString();

        }


        public override string ToString() { return this.toJSON(); }
        public bool isValid()
        {

            var props = typeof(CSZoneDefinition).GetProperties();

            foreach (var prop in props)
            {

                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }




        public static CSZoneDefinition fromJSON(string json)
        {
            return Serialization.Deserialize<CSZoneDefinition>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSZoneDefinition>(this);
        }








        public string buffMe()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, this);
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
        public static CSZoneDefinition unBuffMe( string txt)
        {
            byte[] arr = Convert.FromBase64String(txt);
            using (MemoryStream ms = new MemoryStream(arr))
                return ProtoBuf.Serializer.Deserialize<CSZoneDefinition>(ms);
        }






public CSLibrary getAllLibraryReferences(CSLibrary library)
        {
            var newLib = new CSLibrary();

            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.RoofConstruction));
            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.FacadeConstruction));
            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.SlabConstruction));
            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.PartitionConstruction));
            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.GroundConstruction));
            newLib.Add(library.getElementByName<CSOpaqueConstruction>(this.Materials.InternalMassConstruction));


            HashSet<string> List_usedYearSchedules = new HashSet<string>();

            
            // add zone schedules
            List_usedYearSchedules.Add(this.Loads.OccupancySchedule);
            List_usedYearSchedules.Add(this.Loads.AirspeedSchedule);
            List_usedYearSchedules.Add(this.Conditioning.MechVentSchedule);
            List_usedYearSchedules.Add(this.Ventilation.ScheduledVentilationSchedule);
            List_usedYearSchedules.Add(this.DomHotWater.WaterSchedule);
            List_usedYearSchedules.Add(this.Loads.LightsAvailibilitySchedule);
            List_usedYearSchedules.Add(this.Loads.EquipmentAvailibilitySchedule);
            List_usedYearSchedules.Add(this.Ventilation.NatVentSchedule);
            List_usedYearSchedules.Add(this.Conditioning.HeatingSchedule);
            List_usedYearSchedules.Add(this.Conditioning.CoolingSchedule);
            List_usedYearSchedules.Add(this.Conditioning.HeatingSetpointSchedule);
            List_usedYearSchedules.Add(this.Conditioning.CoolingSetpointSchedule);

                
                foreach (string s in List_usedYearSchedules.ToList())
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        continue;
                    }
                    // check if year schedule is not array schedule 
                    if (library.YearSchedules.Any(o => o.Name == s))
                    {
                        var ys = library.YearSchedules.First(o => o.Name == s);
                        newLib.Add(ys);
                    }
                    else if (library.ArraySchedules.Any(o => o.Name == s))
                    {
                        newLib.Add(library.ArraySchedules.First(o => o.Name == s));
                    }
                }

            return newLib;
        }
    }
}
