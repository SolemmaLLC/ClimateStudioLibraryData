using ArchsimLib.Utilities;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class ZoneLoad : LibraryComponent
    {

        [DataMember]
        [Units("p/m2")]
        [DefaultValue(0.2)]
        public double PeopleDensity { get; set; } = 0.2;

        [DataMember]
        [Units("met")]
        [DefaultValue(1.2)]
        public double MetabolicRate { get; set; } = 1.2;

        [DataMember]
        [Units("m/s")]
        [DefaultValue("AirSpeed 0")]
        public string AirspeedSchedule { get; set; }  = "AirSpeed 0";

        [DataMember]
        [Units("W/m2")]
        [DefaultValue(12.0)]
        public double EquipmentPowerDensity { get; set; } = 12;

        [DataMember]
        [Units("W/m2")]
        [DefaultValue(12.0)]
        public double LightingPowerDensity { get; set; } = 12;

        [DataMember]
        [Units("lux")]
        [DefaultValue(500.0)]
        public double IlluminanceTarget { get; set; } = 500;

        [DataMember]
        [DefaultValue("AllOn")]
        public string OccupancySchedule { get; set; } = "AllOn";

        [DataMember]
        [DefaultValue("AllOn")]
        public string EquipmentAvailibilitySchedule { get; set; } = "AllOn";

        [DataMember]
        [DefaultValue("AllOn")]
        public string LightsAvailibilitySchedule { get; set; } = "AllOn";


        [DataMember]
        [DefaultValue(DimmingItem.Off)]
        public DimmingItem DimmingType { get; set; } = DimmingItem.Off; //"Continuous";


        [DataMember]
        [DefaultValue(true)]
        public bool PeopleIsOn { get; set; } = true;

        [DataMember]
        [DefaultValue(true)]
        public bool EquipmentIsOn { get; set; } = true;

        [DataMember]
        [DefaultValue(true)]
        public bool LightsIsOn { get; set; } = true;

        public ZoneLoad()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
