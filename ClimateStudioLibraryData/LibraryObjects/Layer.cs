using System;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract]
    public class Layer<MaterialT>
    {
        [DataMember]
        public MaterialT Material { get; set; }

        [DataMember][Units("m")]
        public double Thickness { get; set; }



        public Layer(double _Thickness, MaterialT _Material)
        {
            Material = _Material;
            Thickness = _Thickness;
        }


        public bool Correct()
        {
            bool changed = false;
            if (this.Thickness < 0.001) { this.Thickness = 0.001; changed = true; }
            if (this.Thickness > 2) { this.Thickness = 2; changed = true; }
            return changed;
        }

        public string GetMaterialName()
        {
            var mat = Material as LibraryComponent;
            if (mat != null) return mat.Name;
            else return null;
        }
        public void SetMaterialName(string name)
        {
            var mat = Material as LibraryComponent;
            if (mat != null)  mat.Name = name;
        }


        public override string ToString()
        {
            return "Material: " +Material.ToString() + " Thickness: " + Math.Round(Thickness,2) ;
        }
    }



    //[DataContract(IsReference = true)]
    //public class BaseLayer
    //{
    //    [DataMember]
    //    public string GasType = "";
    //    [DataMember]
    //    public double Thickness = 0.01;

    //    public bool Correct()
    //    {
    //        bool changed = false;
    //        if (this.Thickness < 0.001) { this.Thickness = 0.001; changed = true; }
    //        if (this.Thickness > 2) { this.Thickness = 2; changed = true; }
    //        return changed;
    //    }
    //}
    //[DataContract(IsReference = true)]
    //public class OpaqueLayer : BaseLayer
    //{
    //    [DataMember]
    //    public OpaqueMaterial Material = null;
    //    public OpaqueLayer() { }

    //    public OpaqueLayer(double _Thickness, OpaqueMaterial _Material)
    //    {
    //        Material = _Material;
    //        GasType = "OpaqueMaterial";
    //        Thickness = _Thickness;
    //    }
    //}
    //[DataContract(IsReference = true)]
    //public class GlazingLayer : BaseLayer
    //{

    //    [DataMember]
    //    public GlazingMaterial GlassMaterial = null;
    //    [DataMember]
    //    public GasMaterial GasMaterial = null;

    //    public string GetMaterialName()
    //    {
    //        if (GasType == "GlazingMaterial") return GlassMaterial.Name;
    //        else return GasMaterial.Name;
    //    }
    //    public void SetMaterialName(string name)
    //    {
    //        if (GasType == "GlazingMaterial") GlassMaterial.Name = name;
    //        else GasMaterial.Name = name;
    //    }

    //    public GlazingLayer() { }

    //    public GlazingLayer(double _Thickness, GlazingMaterial _Material)
    //    {
    //        GlassMaterial = _Material;
    //        GasType = "GlazingMaterial";
    //        Thickness = _Thickness;
    //    }

    //    public GlazingLayer(double _Thickness, GasMaterial _Material)
    //    {
    //        GasMaterial = _Material;
    //        GasType = "GasMaterial";
    //        Thickness = _Thickness;
    //    }
    //}
}

