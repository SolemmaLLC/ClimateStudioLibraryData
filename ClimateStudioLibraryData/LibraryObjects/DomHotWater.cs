using ArchsimLib.Utilities;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class DomHotWater : LibraryComponent
    {
        [DataMember]
        [Units("C")]
        public double WaterTemperatureInlet { get; set; } = 10;
        [DataMember]
        [Units("C")]
        public double WaterSupplyTemperature { get; set; } = 65;
        [DataMember]
        public string WaterSchedule { get; set; } = "AllOn";
        [DataMember]
        [Units("m3/h/P")]
        public double FlowRatePerPerson { get; set; } = 0.03;
        [DataMember]
        public bool IsOn = false;

        public DomHotWater()
        {
        }
        //public static DomHotWater Deserialize(string xml)
        //{
        //    return (DomHotWater)SerializeDeserialize.Deserialize(xml, typeof(DomHotWater));
        //}
        //public string XMLify()
        //{
        //    return SerializeDeserialize.Serialize(this);
        //}
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
