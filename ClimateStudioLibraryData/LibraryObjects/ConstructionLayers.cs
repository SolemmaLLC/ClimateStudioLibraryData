using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    //---------------------------
    [DataContract] //(IsReference = true)
    [ProtoContract]

    public class OpaqueMaterialAirGap : LibraryComponent//: CSBaseMaterial
    {

        [DataMember, DefaultValue(0.2079491) , Units("m2.K/w")]
        [ProtoMember(1)]
        public double Resistance { get; set; } = 0.2079491;



        [DataMember, DefaultValue(0.0)]
        [Units("MJ/Kg")]
        [ProtoMember(101)]

        public double EmbodiedEnergy { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(102)]

        public double EmbodiedEnergyStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("kgCO2eq/Kg")]
        [ProtoMember(103)]

        public double EmbodiedCarbon { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(104)]

        public double EmbodiedCarbonStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("$/m3")]
        [ProtoMember(105)]

        public double Cost { get; set; } = 0;

        [DataMember, DefaultValue(10)]
        [Units("yr")]
        [ProtoMember(106)]
        public int Life { get; set; } = 10;


        public OpaqueMaterialAirGap() { }
    }

    [DataContract(IsReference = true)]
    [ProtoContract]
    public class OpaqueMaterialNoMass : LibraryComponent //: CSBaseMaterial
    {


        [DataMember , DefaultValue(0.2079491), Units("m2.K/w")]
        [ProtoMember(1)]
        public double Resistance { get; set; } = 0.2079491;

        [DataMember, DefaultValue("Rough")]
        [ProtoMember(2)]
        public string Roughness { get; set; } = "Rough";

        [DataMember, DefaultValue(0.9), Units("0-1")]
        [ProtoMember(3)]
        public double ThermalAbsorptance { get; set; } = 0.9;

        [DataMember, DefaultValue(0.7), Units("0-1")]
        [ProtoMember(4)]
        public double SolarAbsorptance { get; set; } = 0.7;

        [DataMember, DefaultValue(0.7), Units("0-1")]
        [ProtoMember(5)]
        public double VisibleAbsorptance { get; set; } = 0.7;



        [DataMember, DefaultValue(0.0)]
        [Units("MJ/Kg")]
        [ProtoMember(101)]

        public double EmbodiedEnergy { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(102)]

        public double EmbodiedEnergyStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("kgCO2eq/Kg")]
        [ProtoMember(103)]

        public double EmbodiedCarbon { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(104)]

        public double EmbodiedCarbonStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("$/m3")]
        [ProtoMember(105)]

        public double Cost { get; set; } = 0;

        [DataMember, DefaultValue(10)]
        [Units("yr")]
        [ProtoMember(106)]
        public int Life { get; set; } = 10;


        public OpaqueMaterialNoMass() { }
    }

    //---------------------------


    [DataContract]//(IsReference = true)
    [ProtoContract]
    public class CSOpaqueMaterial  :LibraryComponent  //: CSBaseMaterial
    {
 
        [DataMember, DefaultValue(2.4), Units("W/m.K")]
        [ProtoMember(1)]
        public double Conductivity { get; set; } = 2.4;
 
        [DataMember, DefaultValue(2400.0), Units("kg/m3")]
        [ProtoMember(2)]
        public double Density { get; set; } = 2400;

        [DataMember, DefaultValue("Rough")]
        [ProtoMember(3)]
        public string Roughness { get; set; } = "Rough";

        [DataMember, DefaultValue(840.0),  Units("J/kg.K")]
        [ProtoMember(4)]
        public double SpecificHeat { get; set; } = 840;

        [DataMember , DefaultValue(0.9), Units("0-1")]
        [ProtoMember(5)]
        public double ThermalAbsorptance { get; set; } = 0.9;

        [DataMember, DefaultValue(0.7), Units("0-1")]
        [ProtoMember(6)]
        public double SolarAbsorptance { get; set; } = 0.7;
        
        [DataMember, DefaultValue(0.7), Units("0-1")]
        [ProtoMember(7)]
        public double VisibleAbsorptance { get; set; } = 0.7;



        [DataMember, DefaultValue(false)]
        [ProtoMember(8)]
        public bool PhaseChange { get; set; } = false;

        [DataMember, DefaultValue(false)]
        [ProtoMember(9)]
        public bool VariableConductivity { get; set; } = false;

        [DataMember, DefaultValue(0.0), Units("W/m-K2")]
        [ProtoMember(10)]
        public double TemperatureCoefficientThermalConductivity { get; set; } = 0;

        [DataMember,Units("C")]
        [ProtoMember(11)]
        public List<double> TemperatureArray { get; set; } = new List<double>();
        
        
        [DataMember,Units("J/kg")]
        [ProtoMember(12)]
        public List<double> EnthalpyArray { get; set; } = new List<double>();


        [DataMember,Units("W/m-K")]
        [ProtoMember(13)]
        public List<double> VariableConductivityArray { get; set; } = new List<double>();

        [DataMember]
        [ProtoMember(14)]
        public OpaqueMaterialTypes Type { get; set; } = OpaqueMaterialTypes.Other;


        


        /// <summary>
        /// Dimensionless factor µ (DIN EN ISO 12572)
        /// </summary>
        //[DataMember]
        //[Units("Dimensionless")]
        //public double MoistureDiffusionResistance { get; set; } = 50;




        [DataMember, DefaultValue(0.0)]
        [Units("MJ/Kg")]
        [ProtoMember(101)]

        public double EmbodiedEnergy { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(102)]

        public double EmbodiedEnergyStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("kgCO2eq/Kg")]
        [ProtoMember(103)]

        public double EmbodiedCarbon { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [ProtoMember(104)]

        public double EmbodiedCarbonStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0)]
        [Units("$/m3")]
        [ProtoMember(105)]

        public double Cost { get; set; } = 0;

        [DataMember, DefaultValue(10)]
        [Units("yr")]
        [ProtoMember(106)]
        public int Life { get; set; } = 10;


        public CSOpaqueMaterial() { }


        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            if (this.VisibleAbsorptance < 0.001) { this.VisibleAbsorptance = 0.001; changed = true; }
            if (this.VisibleAbsorptance > 0.999) { this.VisibleAbsorptance = 0.999; changed = true; }
            if (this.SolarAbsorptance < 0.001) { this.SolarAbsorptance = 0.001; changed = true; }
            if (this.SolarAbsorptance > 0.999) { this.SolarAbsorptance = 0.999; changed = true; }
            if (this.ThermalAbsorptance < 0.001) { this.ThermalAbsorptance = 0.001; changed = true; }
            if (this.ThermalAbsorptance > 0.999) { this.ThermalAbsorptance = 0.999; changed = true; }
            if (this.SpecificHeat < 100) { this.SpecificHeat = 100; changed = true; }
            if (this.SpecificHeat > 5000) { this.SpecificHeat = 2000; changed = true; }
            if (this.Density < 10) { this.Density = 10; changed = true; }
            if (this.Density > 5000) { this.Density = 5000; changed = true; }
            if (this.Conductivity < 0.001) { this.Conductivity = 0.001; changed = true; }
            if (this.Conductivity > 5000) { this.Conductivity = 5000; changed = true; }

            return changed;
        }

        public static CSOpaqueMaterial fromJSON(string json)
        {
            return Serialization.Deserialize<CSOpaqueMaterial>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSOpaqueMaterial>(this);
        }
    }




    //[ProtoContract]
    //[ProtoInclude(3100, typeof(CSOpaqueMaterial))]
    //[ProtoInclude(3110, typeof(OpaqueMaterialNoMass))]
    //[ProtoInclude(3120, typeof(OpaqueMaterialAirGap))]
    //[ProtoInclude(3200, typeof(CSWindowMaterialBase))]

    //public abstract class CSBaseMaterial : LibraryComponent
    //{
    //    [DataMember, DefaultValue(0.0)]
    //    [Units("MJ/Kg")]
    //    [ProtoMember(101)]

    //    public double EmbodiedEnergy { get; set; } = 0;

    //    [DataMember, DefaultValue(0.0)]
    //    [ProtoMember(102)]

    //    public double EmbodiedEnergyStdDev { get; set; } = 0;

    //    [DataMember, DefaultValue(0.0)]
    //    [Units("kgCO2eq/Kg")]
    //    [ProtoMember(103)]

    //    public double EmbodiedCarbon { get; set; } = 0;

    //    [DataMember, DefaultValue(0.0)]
    //    [ProtoMember(104)]

    //    public double EmbodiedCarbonStdDev { get; set; } = 0;

    //    [DataMember, DefaultValue(0.0)]
    //    [Units("$/m3")]
    //    [ProtoMember(105)]

    //    public double Cost { get; set; } = 0;

    //    [DataMember, DefaultValue(10)]
    //    [Units("yr")]
    //    [ProtoMember(106)]
    //    public int Life { get; set; } = 10;

    //    public CSBaseMaterial() { }


    //}


    [DataContract]
    [ProtoContract]

    public class Layer<MaterialT>
    {
        [DataMember]
        [ProtoMember(1, AsReference = true)]
        public MaterialT Material { get; set; }

        [DataMember,Units("m")]
        [ProtoMember(2)]
        public double Thickness { get; set; }

        public Layer(double _Thickness, MaterialT _Material)
        {
            Material = _Material;
            Thickness = _Thickness;
        }

        public Layer() { }

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
            if (mat != null) mat.Name = name;
        }


        public override string ToString()
        {
            return "Material: " + Material.ToString() + " Thickness: " + Math.Round(Thickness, 2);
        }
    }
}
