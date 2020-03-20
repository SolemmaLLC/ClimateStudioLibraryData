using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    [ProtoContract]

    public class CSGlazingMaterial : CSWindowMaterialBase
    {
        /// <summary>
        /// Conductivity {W/m.K}
        /// </summary>
        [DataMember]
        [Units("W/m.K")]
        [ProtoMember(1)]
        public double Conductivity { get; set; } = 0;
        /// <summary>
        /// Density {kg/m3}
        /// </summary>
        [DataMember]
        [Units("kg/m3")]
        [ProtoMember(2)]
        public double Density { get; set; } = 2500;
        /// <summary>
        /// Optical data type {SpectralAverage or Spectral}
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public string Optical { get; set; } = "SpectralAverage";
       
        
        /// <summary>
        /// Name of spectral data set when Optical Data Type = Spectral
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public string OpticalDataName { get; set; } = "";

        // IGDB info
        [DataMember]
        [ProtoMember(5)]
        public int NFRC_ID { get; set; }


        [DataMember]
        [ProtoMember(6)]
        public int Glazing_ID { get; set; }


        [DataMember]
        [ProtoMember(7)]
        public string CoatingSide { get; set; }



        [DataMember]
        [Units("Microns")]
        [ProtoMember(8, OverwriteList = true)]
        public List<double> SpectralDataPointWavelength { get; set; } = new List<double>();


        [DataMember]
        [Units("0-1")]
        [ProtoMember(9, OverwriteList = true)]
        public List<double> SpectralDataPointTransmittance { get; set; } = new List<double>();


        [DataMember]
        [Units("0-1")]
        [ProtoMember(10, OverwriteList = true)]
        public List<double> SpectralDataPointFrontReflectance { get; set; } = new List<double>();


        [DataMember]
        [Units("0-1")]
        [ProtoMember(11, OverwriteList = true)]
        public List<double> SpectralDataPointBackReflectance { get; set; } = new List<double>();





        /// <summary>
        /// Solar transmittance at normal incidence
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(12)]
        public double SolarTransmittance { get; set; } = 0.837;


        /// <summary>
        /// Solar reflectance at normal incidence: front side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(13)]
        public double SolarReflectanceFront { get; set; } = 0.075;


        /// <summary>
        /// Solar reflectance at normal incidence: back side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(14)]
        public double SolarReflectanceBack { get; set; } = 0.075;


        /// <summary>
        /// Visible transmittance at normal incidence
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(15)]
        public double VisibleTransmittance { get; set; } = 0.898;


        /// <summary>
        /// Visible reflectance at normal incidence: front side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(16)]
        public double VisibleReflectanceFront { get; set; } = 0.081;


        /// <summary>
        /// Visible reflectance at normal incidence: back side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(17)]
        public double VisibleReflectanceBack { get; set; } = 0.081;


        /// <summary>
        /// IR transmittance at normal incidence
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(18)]
        public double IRTransmittance { get; set; } = 0.0;


        /// <summary>
        /// IR emissivity: front side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(19)]
        public double IREmissivityFront { get; set; } = 0.84;


        /// <summary>
        /// IR emissivity: back side
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(20)]
        public double IREmissivityBack { get; set; } = 0.84;


        /// <summary>
        /// Dirt Correction Factor for Solar and Visible Transmittance
        /// </summary>
        [DataMember]
        [Units("0-1")]
        [ProtoMember(21)]
        public double DirtFactor { get; set; } = 1;

        [DataMember]
         [ProtoMember(22)]
        public string Type { get; set; } = "Glass";




        public CSGlazingMaterial() { }

        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }


            if (this.SolarTransmittance < 0.0) { this.SolarTransmittance = 0.0; changed = true; }
            if (this.SolarTransmittance > 1.0) { this.SolarTransmittance = 1.0; changed = true; }

            if (this.SolarReflectanceFront < 0.0) { this.SolarReflectanceFront = 0.0; changed = true; }
            if (this.SolarReflectanceFront > 1.0) { this.SolarReflectanceFront = 1.0; changed = true; }

            if (this.SolarReflectanceBack < 0.0) { this.SolarReflectanceBack = 0.0; changed = true; }
            if (this.SolarReflectanceBack > 1.0) { this.SolarReflectanceBack = 1.0; changed = true; }

            if (this.VisibleTransmittance < 0.0) { this.VisibleTransmittance = 0.0; changed = true; }
            if (this.VisibleTransmittance > 1.0) { this.VisibleTransmittance = 1.0; changed = true; }

            if (this.VisibleReflectanceFront < 0.0) { this.VisibleReflectanceFront = 0.0; changed = true; }
            if (this.VisibleReflectanceFront > 1.0) { this.VisibleReflectanceFront = 1.0; changed = true; }

            if (this.VisibleReflectanceBack < 0.0) { this.VisibleReflectanceBack = 0.0; changed = true; }
            if (this.VisibleReflectanceBack > 1.0) { this.VisibleReflectanceBack = 1.0; changed = true; }

            if (this.IRTransmittance < 0.0) { this.IRTransmittance = 0.0; changed = true; }
            if (this.IRTransmittance > 1.0) { this.IRTransmittance = 1.0; changed = true; }

            if (this.IREmissivityFront < 0.0) { this.IREmissivityFront = 0.0; changed = true; }
            if (this.IREmissivityFront > 1.0) { this.IREmissivityFront = 1.0; changed = true; }

            if (this.IREmissivityBack < 0.0) { this.IREmissivityBack = 0.0; changed = true; }
            if (this.IREmissivityBack > 1.0) { this.IREmissivityBack = 1.0; changed = true; }

            if (this.Density < 10) { this.Density = 10; changed = true; }
            if (this.Density > 5000) { this.Density = 5000; changed = true; }

            if (this.Conductivity < 0.001) { this.Conductivity = 0.001; changed = true; }
            if (this.Conductivity > 5000) { this.Conductivity = 5000; changed = true; }

            return changed;
        }

        public static CSGlazingMaterial fromJSON(string json)
        {
            return Serialization.Deserialize<CSGlazingMaterial>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSGlazingMaterial>(this);
        }
    }


    public enum GasTypes
    {
        AIR,
        ARGON,
        KRYPTON,
        XENON,
        SF6,
        CUSTOM
    };

    public enum GasModelType
    {
        Gas,
        Custom,
        GasMixture 
    };



    [DataContract(IsReference = true)]
    [ProtoContract]
    public class CSGasMaterial : CSWindowMaterialBase
    {
        [DataMember]
        [ProtoMember(1)]
        public GasModelType Model { get; set; } = GasModelType.Gas;




        [DataMember]
        [ProtoMember(2)]
        public GasTypes GasType1 { get; set; } = GasTypes.AIR;



        // USED IF Model is GasMix
        [DataMember]
        [ProtoMember(3)]
        public int GasesInMix { get; set; } = 1;



        [DataMember]
        [ProtoMember(4)]
        public GasTypes GasType2 { get; set; } = GasTypes.AIR;




        [DataMember]
        [ProtoMember(5)]
        public GasTypes GasType3 { get; set; } = GasTypes.AIR;



        [DataMember]
        [ProtoMember(6)]
        public double Ratio1 { get; set; } = 1;



        [DataMember]
        [ProtoMember(7)]
        public double Ratio2 { get; set; } = 0;


        [DataMember]
        [ProtoMember(8)]
        public double Ratio3 { get; set; } = 0;


        // USED IF GasType1 is set to Custom


        [DataMember]
        [ProtoMember(9)]
        public double ConductivityCoefficientA { get; set; } 


        [DataMember]
        [ProtoMember(10)]
        public double ConductivityCoefficientB { get; set; }



        [DataMember]
        [ProtoMember(11)]
        public double ConductivityCoefficientC { get; set; }




        [DataMember]
        [ProtoMember(12)]
        public double MolecularWeight { get; set; }


        [DataMember]
        [ProtoMember(13)]
        public double SpecificHeatCoefficientA { get; set; }



        [DataMember]
        [ProtoMember(14)]
        public double SpecificHeatCoefficientB { get; set; }



        [DataMember]
        [ProtoMember(15)]
        public double SpecificHeatCoefficientC { get; set; }



        [DataMember]
        [ProtoMember(16)]
        public double SpecificHeatRatio { get; set; }



        [DataMember]
        [ProtoMember(17)]
        public double ViscosityCoefficientA { get; set; }


        [DataMember]
        [ProtoMember(18)]
        public double ViscosityCoefficientB { get; set; }


        [DataMember]
        [ProtoMember(19)]
        public double ViscosityCoefficientC { get; set; }






        public CSGasMaterial()
        {
         }
        public CSGasMaterial(GasTypes _Type, double thickness)
        {
            GasType1 = _Type;
            Name = Name = GasType1.ToString() + "_" + thickness;
            Thickness = thickness;
        }

        public static CSGasMaterial fromJSON(string json)
        {
            return Serialization.Deserialize<CSGasMaterial>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSGasMaterial>(this);
        }
    }


    [DataContract(IsReference = true)]
    [KnownType(typeof(CSGasMaterial))]
    [KnownType(typeof(CSGlazingMaterial))]

    [ProtoContract]
    [ProtoInclude(3300, typeof(CSGlazingMaterial))]
    [ProtoInclude(3400, typeof(CSGasMaterial))]

    public abstract class CSWindowMaterialBase : LibraryComponent //: CSBaseMaterial
    {
        [DataMember]
        [Units("m")]
        [ProtoMember(1)]
        public double Thickness { get; set; }


        [DataMember, DefaultValue(0.0)]
        [Units("MJ/Kg")]
        [ProtoMember(101)]
        public double EmbodiedEnergy { get; set; } = 15;


        [DataMember, DefaultValue(0.0)]
        [ProtoMember(102)]
        public double EmbodiedEnergyStdDev { get; set; } = 0;


        [DataMember, DefaultValue(0.0)]
        [Units("kgCO2eq/Kg")]
        [ProtoMember(103)]
        public double EmbodiedCarbon { get; set; } = 0.91;


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

    }

    //public class SpectralDataPoint {
    //    [DataMember]
    //    [Units("Microns")]
    //    public double Wavelength { get; set; } = 0;
 
    //    [DataMember]
    //    [Units("0-1")]
    //    public double Transmittance { get; set; } = 0;
       
    //    [DataMember]
    //    [Units("0-1")]
    //    public double FrontReflectance { get; set; } = 0;
       
    //    [DataMember]
    //    [Units("0-1")]
    //    public double BackReflectance { get; set; } = 0;

    //}

}
