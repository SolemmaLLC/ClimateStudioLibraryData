using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract ]
    [ProtoContract]

    public class CSZoneHotWater : LibraryComponent
    {
        [DataMember, DefaultValue(1.0)]
        [ProtoMember(1)]
        public double DomHotWaterCOP { get; set; } = 1;


        [DataMember, DefaultValue(10.0)]
        [Units("C")]
        [ProtoMember(2)]
        public double WaterTemperatureInlet { get; set; } = 10;


        [DataMember, DefaultValue(65.0)]
        [Units("C")]
        [ProtoMember(3)]
        public double WaterSupplyTemperature { get; set; } = 65;


        [DataMember, DefaultValue("AllOn")]
        [ProtoMember(4)]
        public string WaterSchedule { get; set; } = "AllOn";


        [DataMember, DefaultValue(0.03)]
        [Units("m3/h/P")]
        [ProtoMember(5)]
        public double FlowRatePerPerson { get; set; } = 0.03;

        [DataMember, DefaultValue(false)]
        [ProtoMember(6)]
        public bool IsOn { get; set; } = false;


        


        public CSZoneHotWater()
        {
        }
 
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
