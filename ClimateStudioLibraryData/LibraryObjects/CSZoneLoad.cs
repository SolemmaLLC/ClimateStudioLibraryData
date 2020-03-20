using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract ]
    [ProtoContract]
    public class CSZoneLoad : LibraryComponent
    {
        [DataMember]
        [DefaultValue("Office")]
        [ProtoMember(1)]

        public string Program { get; set; } = "Office";


        [DataMember]
        [DefaultValue(ZoneUseTypeEnum.MediumOffice)]
        [ProtoMember(15)]

        public ZoneUseTypeEnum ZoneUseType { get; set; } = ZoneUseTypeEnum.MediumOffice;




        [DataMember]
        [Units("p/m2")]
        [DefaultValue(0.2)]
        [ProtoMember(2)]

        public double PeopleDensity { get; set; } = 0.2;



        [DataMember]
        [Units("met")]
        [DefaultValue(1.2)]
        [ProtoMember(3)]
        public double MetabolicRate { get; set; } = 1.2;



        [DataMember]
        [Units("m/s")]
        [DefaultValue("AirSpeed 0")]
        [ProtoMember(4)]

        public string AirspeedSchedule { get; set; }  = "AirSpeed 0";

        [DataMember]
        [Units("W/m2")]
        [DefaultValue(12.0)]
        [ProtoMember(5)]
        public double EquipmentPowerDensity { get; set; } = 12;



        [DataMember]
        [Units("W/m2")]
        [DefaultValue(12.0)]
        [ProtoMember(6)]
        public double LightingPowerDensity { get; set; } = 12;



        [DataMember]
        [Units("lux")]
        [DefaultValue(500.0)]
        [ProtoMember(7)]
        public double IlluminanceTarget { get; set; } = 500;



        [DataMember]
        [DefaultValue("AllOn")]
        [ProtoMember(8)]
        public string OccupancySchedule { get; set; } = "AllOn";



        [DataMember]
        [DefaultValue("AllOn")]
        [ProtoMember(9)]
        public string EquipmentAvailibilitySchedule { get; set; } = "AllOn";



        [DataMember]
        [DefaultValue("AllOn")]
        [ProtoMember(10)]
        public string LightsAvailibilitySchedule { get; set; } = "AllOn";


        [DataMember]
        [DefaultValue(DimmingItem.Off)]
        [ProtoMember(11)]
        public DimmingItem DimmingType { get; set; } = DimmingItem.Off; //"Continuous";


        [DataMember]
        [DefaultValue(true)]
        [ProtoMember(12)]
        public bool PeopleIsOn { get; set; } = true;



        [DataMember]
        [DefaultValue(true)]
        [ProtoMember(13)]
        public bool EquipmentIsOn { get; set; } = true;

        [DataMember]
        [DefaultValue(true)]
        [ProtoMember(14)]

        public bool LightsIsOn { get; set; } = true;



        



        public CSZoneLoad()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
