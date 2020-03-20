using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract ]
    [ProtoContract]

    public class CSZoneConstruction : LibraryComponent
    {
        public CSZoneConstruction() { }
        public override string ToString() { return Serialization.Serialize(this); }
        public bool isValid()
        {

            var props = typeof(CSZoneConstruction).GetProperties();

            foreach (var prop in props)
            {
                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }



        [DataMember,DefaultValue("defaultConstruction")]
        [ProtoMember(1)]
        public string RoofConstruction { get; set; } = "defaultConstruction";


        [DataMember,DefaultValue("defaultConstruction")]
        [ProtoMember(2)]
        public string FacadeConstruction { get; set; } = "defaultConstruction";


        [DataMember,DefaultValue("defaultConstruction")]
        [ProtoMember(3)]
        public string SlabConstruction { get; set; } = "defaultConstruction";


        [DataMember,DefaultValue("defaultConstruction")]
        [ProtoMember(4)]
        public string PartitionConstruction { get; set; } = "defaultConstruction";


        [DataMember,DefaultValue("defaultConstruction")]
        [ProtoMember(5)]
        public string GroundConstruction { get; set; } = "defaultConstruction";


        [DataMember,DefaultValue(false)]
        [ProtoMember(6)]
        public bool GroundIsAdiabatic { get; set; } = false;


        [DataMember,DefaultValue(false)]
        [ProtoMember(7)]
        public bool RoofIsAdiabatic { get; set; } = false;


        [DataMember,DefaultValue(false)]
        [ProtoMember(8)]
        public bool FacadeIsAdiabatic { get; set; } = false;


        [DataMember,DefaultValue(false)]
        [ProtoMember(9)]
        public bool SlabIsAdiabatic { get; set; } = false;


        [DataMember,DefaultValue(false)]
        [ProtoMember(10)]
        public bool PartitionIsAdiabatic { get; set; } = false;





        // use this istead of PartritionRatio
        [DataMember]
        [DefaultValue("defaultConstruction")]
        [ProtoMember(11)]
        public string InternalMassConstruction { get; set; } = "defaultConstruction";


        // use this istead of PartritionRatio
        [DataMember]
        [DefaultValue(0.0)]
        [ProtoMember(12)]
        public double InternalMassExposedAreaPerArea { get; set; } = 0;



        [DataMember]
        [DefaultValue(InConvAlgo.TARP)]
        [ProtoMember(13)]
        public InConvAlgo SurfaceConvectionModelInside { get; set; } = InConvAlgo.TARP;

        [DataMember]
        [DefaultValue(OutConvAlgo.DOE2)]
        [ProtoMember(14)]
        public OutConvAlgo SurfaceConvectionModelOutside { get; set; } = OutConvAlgo.DOE2;



        [DataMember]
        [DefaultValue(1)]
        [ProtoMember(15)]
        public int ZonePriority { get; set; } = 1;

        [DataMember]
        [DefaultValue(1.0)]
        [ProtoMember(16)]
        public double DaylightMeshResolution { get; set; } = 1.0;

        [DataMember]
        [DefaultValue(0.8)]
        [ProtoMember(17)]
        public double DaylightWorkplaneHeight { get; set; } = 0.8;

        

    }
}
