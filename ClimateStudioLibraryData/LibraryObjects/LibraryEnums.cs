namespace ArchsimLib.LibraryObjects
{
    //public enum InfiltrationModel { Constant, Wind };
    public enum AFNVentilationControl { Temperature, Constant };
    public enum ShadingType { ExteriorShade, InteriorShade };
    public enum DimmingItem { Off, Stepped, Continuous };
    public enum HeatRecoveryItem { None, Sensible, Enthalpy };
    public enum EconomizerItem { NoEconomizer, DifferentialDryBulb, DifferentialEnthalpy };
    public enum IdealSystemLimit { NoLimit, LimitFlowRate, LimitCapacity, LimitFlowRateAndCapacity };
    public enum InConvAlgo { Simple, TARP, TrombeWall, AdaptiveConvectionAlgorithm };
    public enum OutConvAlgo { DOE2, TARP, MoWiTT, SimpleCombined, AdaptiveConvectionAlgorithm }; //  DOE-2,  
    public enum windowType { External, Internal };
    public enum ShadingControlType { OnIfScheduleAllows, OnIfHighSolarOnWindow , OnIfHighHorizontalSolar, OnIfHighOutdoorAirTemperature, OnNightIfLowOutdoorTempAndOnDayIfCooling }


    public enum ScheduleType { Fraction, Temperature , AnyNumber};

    public enum ScheduleCategory
    {
        Control,Occupancy,Availability,Equipment,Lights,AirSpeed 
    };


    public enum HeatingCoolingModes
    {  ConstantSetpoint, ScheduledSetpoint ,Off }

public enum ConstructionCategory
    {
        Facade,
        Roof,
        GroundFloor,
        InteriorFloor,
        ExteriorFloor,
        Partition
    };
    public enum GlazingConstructionTypes
    {
        Other,
        Single,
        Double,
        Triple,
        Quadruple
    };





   

    public enum EconomizerItemHVACTemp {  
FixedDryBulb,
DifferentialDryBulb,
FixedEnthalpy,
DifferentialEnthalpy,
ElectronicEnthalpy,
FixedDewPointAndDryBulb,
DifferentialDryBulbAndEnthalpy,
NoEconomizer
            };


    public enum FanPartLoadPowerCoefficients { 
InletVaneDampers,
OutletDampers,
VariableSpeedMotor,
//ASHRAE90.1-2004Appendix G,
VariableSpeedMotorPressureReset,
    }


    public enum DehumidificationControlType
    {
        None,
        CoolReheat
    }
    public enum HumidifierType
    {
        None,
 ElectricSteam
    }

    



}
