using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class FloorDefinition : LibraryComponent
    {
        [DataMember]
        public string Type = "INT";
        [DataMember]
        public string BuildingID = "Default";
        [DataMember]
        public double NorthWWR = 0.5;
        [DataMember]
        public double EastWWR = 0.5;
        [DataMember]
        public double SouthWWR = 0.5;
        [DataMember]
        public double WestWWR = 0.5;
        [DataMember]
        public double RoofWWR = 0.5;


        [DataMember]
        public double NorthOverhang = 0;
        [DataMember]
        public double EastOverhang = 0;
        [DataMember]
        public double SouthOverhang = 0;
        [DataMember]
        public double WestOverhang = 0;
        [DataMember]
        public double NorthWingwall = 0;
        [DataMember]
        public double EastWingwall = 0;
        [DataMember]
        public double SouthWingwall = 0;
        [DataMember]
        public double WestWingwall = 0;





        [DataMember]
        public string NorthWindowDefinition = "";
        [DataMember]
        public string EastWindowDefinition = "";
        [DataMember]
        public string SouthWindowDefinition = "";
        [DataMember]
        public string WestWindowDefinition = "";
        [DataMember]
        public string RoofWindowDefinition = "";

        [DataMember]
        public bool isBasement = false;

        [DataMember]
        public string ZoneDefinition { get; set; } = "";


        // this can be used to override the zone construction
        // TODO: reorganize data so that there is no more need to override
        [DataMember]
        public string ZoneConstruction { get; set; } = "";



    }



    [DataContract(IsReference = true)]
    public class BuildingDefinition //: LibraryComponent
    {
        [DataMember]
        public string Name = "Default";
        [DataMember]
        public List<FloorDefinition> Floors = new List<FloorDefinition>();

    }
}


