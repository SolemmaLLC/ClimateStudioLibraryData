using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSEnergyLib.LibraryObjects
{

    public enum SlatOrientation { Horizontal, Vertical };
    public enum ReflectedBeamTransmittanceAccountingMethod { DoNotModel, Empty, ModelAsDiffuse, ModelAsDirectBeam };

    public partial class WindowMaterialBlind
    {
        public double BackSideSlatBeamSolarReflectance { get; set; }

        public double BackSideSlatBeamVisibleReflectance { get; set; }

        public double BackSideSlatDiffuseSolarReflectance { get; set; }

        public double BackSideSlatDiffuseVisibleReflectance { get; set; }

        public double BackSideSlatInfraredHemisphericalEmissivity { get; set; }

        public double BlindBottomOpeningMultiplier { get; set; }

        public double BlindLeftSideOpeningMultiplier { get; set; }

        public double BlindRightSideOpeningMultiplier { get; set; }

        public double BlindToGlassDistance { get; set; }

        public double BlindTopOpeningMultiplier { get; set; }

        public double FrontSideSlatBeamSolarReflectance { get; set; }

        public double FrontSideSlatBeamVisibleReflectance { get; set; }

        public double FrontSideSlatDiffuseSolarReflectance { get; set; }

        public double FrontSideSlatDiffuseVisibleReflectance { get; set; }

        public double FrontSideSlatInfraredHemisphericalEmissivity { get; set; }

        public double MaximumSlatAngle { get; set; }

        public double MinimumSlatAngle { get; set; }

        public double SlatAngle { get; set; }

        public double SlatBeamSolarTransmittance { get; set; }

        public double SlatBeamVisibleTransmittance { get; set; }

        public double SlatConductivity { get; set; }

        public double SlatDiffuseSolarTransmittance { get; set; }

        public double SlatDiffuseVisibleTransmittance { get; set; }

        public double SlatInfraredHemisphericalTransmittance { get; set; }

        public SlatOrientation SlatOrientation { get; set; }

        public double SlatSeparation { get; set; }

        public double SlatThickness { get; set; }

        public double SlatWidth { get; set; }
    }





    public partial class WindowMaterialShade
    {
        public double AirflowPermeability { get; set; }

        public double BottomOpeningMultiplier { get; set; }

        public double Conductivity { get; set; }

        public double InfraredHemisphericalEmissivity { get; set; }

        public double InfraredTransmittance { get; set; }

        public double LeftSideOpeningMultiplier { get; set; }

        public double RightSideOpeningMultiplier { get; set; }

        public double ShadeToGlassDistance { get; set; }

        public double SolarReflectance { get; set; }

        public double SolarTransmittance { get; set; }

        public double Thickness { get; set; }

        public double TopOpeningMultiplier { get; set; }

        public double VisibleReflectance { get; set; }

        public double VisibleTransmittance { get; set; }


    }





    public partial class WindowMaterialScreen
    {
        public double AngleOfResolutionForScreenTransmittanceOutputMap { get; set; } = 0; // Angle of Resolution for Output Map {deg}

        public double BottomOpeningMultiplier { get; set; }

        public double Conductivity { get; set; }

        public double DiffuseSolarReflectance { get; set; }

        public double DiffuseVisibleReflectance { get; set; }

        public double LeftSideOpeningMultiplier { get; set; }

        public ReflectedBeamTransmittanceAccountingMethod ReflectedBeamTransmittanceAccountingMethod { get; set; } = ReflectedBeamTransmittanceAccountingMethod.ModelAsDiffuse;

        public double RightSideOpeningMultiplier { get; set; }

        public double ScreenMaterialDiameter { get; set; }

        public double ScreenMaterialSpacing { get; set; }

        public double ScreenToGlassDistance { get; set; }

        public double ThermalHemisphericalEmissivity { get; set; }

        public double TopOpeningMultiplier { get; set; }
    }

}
