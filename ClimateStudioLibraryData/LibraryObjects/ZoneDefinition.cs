using ArchsimLib.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class ZoneDefinition : LibraryComponent
    {


        [DataMember, DefaultValue(1.0)]
        public double ZoneMultiplier { get; set; } = 1.0;

        [DataMember, DefaultValue(45), Units("deg")]
        public double RoofTiltAngle { get; set; } = 45;

        [DataMember, DefaultValue(135), Units("deg")]
        public double FloorTiltAngle { get; set; } = 135;

        //MAT
        //-----
        [DataMember]
        public ZoneConstruction Materials { get; set; } = new ZoneConstruction();

        //LOADS
        //-----
        [DataMember]
        public ZoneLoad Loads { get; set; } = new ZoneLoad();

        //IdealLoadsAirSystem
        //-------------------
        [DataMember]
        public ZoneConditioning Conditioning { get; set; } = new ZoneConditioning();

        //DOM HOT WAT
        [DataMember]
        public DomHotWater DomHotWater { get; set; } = new DomHotWater();

        //AIRFLOW / VENT
        //--------------
        [DataMember]
        public ZoneVentilation Ventilation { get; set; } = new ZoneVentilation();


        public ZoneDefinition()
        {
        }

        public static ZoneDefinition Clone(ZoneDefinition zsc)
        {
            string s = zsc.toJSON();
            return ZoneDefinition.fromJSON(s);
        }

        public override string ToString() { return this.Serialize(); }
        public bool isValid()
        {

            var props = typeof(ZoneDefinition).GetProperties();

            foreach (var prop in props)
            {

                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }




        public static ZoneDefinition fromJSON(string json)
        {
            return Serialization.Deserialize<ZoneDefinition>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<ZoneDefinition>(this);
        }

    }
}
