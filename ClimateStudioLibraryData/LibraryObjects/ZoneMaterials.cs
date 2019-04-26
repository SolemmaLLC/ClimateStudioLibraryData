using ArchsimLib.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class ZoneConstruction : LibraryComponent
    {
        public ZoneConstruction() { }


        public override string ToString() { return Serialization.Serialize(this); }
        public bool isValid()
        {

            var props = typeof(ZoneConstruction).GetProperties();

            foreach (var prop in props)
            {
                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }

        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string RoofConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string FacadeConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string SlabConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string PartitionConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string GroundConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        [DefaultValue(false)]
        public bool GroundIsAdiabatic { get; set; } = false;
        [DataMember]
        [DefaultValue(false)]
        public bool RoofIsAdiabatic { get; set; } = false;
        [DataMember]
        [DefaultValue(false)]
        public bool FacadeIsAdiabatic { get; set; } = false;
        [DataMember]
        [DefaultValue(false)]
        public bool SlabIsAdiabatic { get; set; } = false;
        [DataMember]
        [DefaultValue(false)]
        public bool PartitionIsAdiabatic { get; set; } = false;



        // use this istead of PartritionRatio
        [DataMember]
        [DefaultValue("defaultConstruction")]
        public string InternalMassConstruction { get; set; } = "defaultConstruction";
        // use this istead of PartritionRatio
        [DataMember]
        [DefaultValue(0.0)]
        public double InternalMassExposedAreaPerArea { get; set; } = 0;



        [DataMember]
        [DefaultValue(InConvAlgo.TARP)]
        public InConvAlgo SurfaceConvectionModelInside { get; set; } = InConvAlgo.TARP;

        [DataMember]
        [DefaultValue(OutConvAlgo.DOE2)]
        public OutConvAlgo SurfaceConvectionModelOutside { get; set; } = OutConvAlgo.DOE2;



        [DataMember]
        [DefaultValue(1)]
        public int ZonePriority { get; set; } = 1;

        [DataMember]
        [DefaultValue(1.0)]
        public double DaylightMeshResolution { get; set; } = 1.0;

        [DataMember]
        [DefaultValue(0.8)]
        public double DaylightWorkplaneHeight { get; set; } = 0.8;

    }
}
