using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{

    [ProtoContract]
    [DataContract(IsReference = true)]
    public class FloorDefinition : LibraryComponent
    {

        public  FloorDefinition() {  }


        [DataMember]
        [ProtoMember(1)]
        public string Type { get; set; } = "INT";
        [DataMember]
        [ProtoMember(2)]
        public string BuildingID { get; set; } = "Default";
        [DataMember]
        [ProtoMember(3)]
        public double NorthWWR { get; set; } = 0.5;
        [DataMember]
        [ProtoMember(4)]
        public double EastWWR { get; set; } = 0.5;
        [DataMember]
        [ProtoMember(5)]
        public double SouthWWR { get; set; } = 0.5;
        [DataMember]
        [ProtoMember(6)]
        public double WestWWR { get; set; } = 0.5;
        [DataMember]
        [ProtoMember(7)]
        public double RoofWWR { get; set; } = 0.5;


        [DataMember]
        [ProtoMember(10)]
        public double NorthOverhang { get; set; } = 0;
        [DataMember]
        [ProtoMember(11)]
        public double EastOverhang { get; set; } = 0;
        [DataMember]
        [ProtoMember(12)]
        public double SouthOverhang { get; set; } = 0;
        [DataMember]
        [ProtoMember(13)]
        public double WestOverhang { get; set; } = 0;
        [DataMember]
        [ProtoMember(14)]
        public double NorthWingwall { get; set; } = 0;
        [DataMember]
        [ProtoMember(15)]
        public double EastWingwall { get; set; } = 0;
        [DataMember]
        [ProtoMember(16)]
        public double SouthWingwall { get; set; } = 0;
        [DataMember]
        [ProtoMember(17)]
        public double WestWingwall { get; set; } = 0;





        [DataMember]
        [ProtoMember(19)]
        public string NorthWindowDefinition { get; set; } = "";
        [DataMember]
        [ProtoMember(20)]
        public string EastWindowDefinition { get; set; } = "";
        [DataMember]
        [ProtoMember(21)]
        public string SouthWindowDefinition { get; set; } = "";
        [DataMember]
        [ProtoMember(22)]
        public string WestWindowDefinition { get; set; } = "";
        [DataMember]
        [ProtoMember(23)]
        public string RoofWindowDefinition { get; set; } = "";

        [DataMember]
        [ProtoMember(24)]
        public bool isBasement { get; set; } = false;

        [DataMember]
        [ProtoMember(25)]
        public string ZoneDefinition { get; set; } = "";


        // this can be used to override the zone construction
        // TODO: reorganize data so that there is no more need to override
        [DataMember]
        [ProtoMember(26)]
        public string ZoneConstruction { get; set; } = "";



    }


    [ProtoContract]
    [DataContract(IsReference = true)]
    public class BuildingDefinition : LibraryComponent
    {

        public BuildingDefinition() { }

        [DataMember]
        [ProtoMember(1,AsReference =true, OverwriteList =true)]

        public List<FloorDefinition> Floors { get; set; } = new List<FloorDefinition>();

    }
}


