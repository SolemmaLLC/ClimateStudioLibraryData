using CSEnergyLib.Utilities;
using CsvHelper.Configuration.Attributes;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{

    public class Units : Attribute
    {
        public Units(string _unit) {
            Unit = _unit;
        }
        public string Unit { get; set; }
    }


    [DataContract]
    [ProtoContract]
    //Zone level
    [ProtoInclude(1000, typeof(CSZoneDefinition))]
    [ProtoInclude(1100, typeof(CSZoneLoad))]
    [ProtoInclude(1200, typeof(CSZoneConditioning))]
    [ProtoInclude(1300, typeof(CSZoneVentilation))]
    [ProtoInclude(1400, typeof(CSZoneHotWater))]
    [ProtoInclude(1500, typeof(CSZoneConstruction))]
    //Window level 
    [ProtoInclude(2000, typeof(CSWindowDefinition))]
    //Materials Constructions
    //[ProtoInclude(3000, typeof(CSBaseMaterial))]
    [ProtoInclude(3100, typeof(CSOpaqueMaterial))]
    [ProtoInclude(3110, typeof(OpaqueMaterialNoMass))]
    [ProtoInclude(3120, typeof(OpaqueMaterialAirGap))]
    [ProtoInclude(3200, typeof(CSWindowMaterialBase))]

    [ProtoInclude(4000, typeof(CSBaseConstruction))]
    //Schedules
    [ProtoInclude(5000, typeof(CSDaySchedule))]
    [ProtoInclude(5100, typeof(CSYearSchedule))]
    [ProtoInclude(5200, typeof(CSArraySchedule))]
    
    
    ////Settings
    //[ProtoInclude(6000, typeof(CSEnergyModelSettings))]

    //[ProtoInclude(7000, typeof(FloorDefinition))]
    //[ProtoInclude(7001, typeof(BuildingDefinition))]

    public class LibraryComponent
   {
        public LibraryComponent() { }

        [DataMember, DefaultValue("No name")]
        [ProtoMember(1)]
        public string Name { get; set; } = "No name";

        [DataMember, DefaultValue("No Category")]
        [ProtoMember(2)]
        public string Category { get; set; } = "No Category";

        [DataMember, DefaultValue("No comments")]
        [ProtoMember(3)]

        public string Comment { get; set; } = "No comments";

        [DataMember, DefaultValue("No data source")]
        [ProtoMember(4)]

        public string DataSource { get; set; } = "No data source";





        //This should be set at runtime and determines what gets saved and what is not
       // [Ignore] // these are ignored by CSVHelper ...
        public bool IsLocked { get; set; } = false;


        //[Ignore]
        public string LibraryName { get; set; } = "";


       //[Ignore]
        public bool IsDefault { get; set; } = false;







        public override string ToString()
        {
            return  Name;
        }

    }
}
