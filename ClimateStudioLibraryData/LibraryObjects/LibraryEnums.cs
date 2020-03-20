//using Eto.Drawing;
using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [ProtoContract]
    public enum ZoneUseTypeEnum
    {

        [EnumMember(Value = "Bank/Financial Institution")] BankBranch,
        [EnumMember(Value = "Courthouse")] Courthouse,
        [EnumMember(Value = "Data Center")] DataCenter,
        [EnumMember(Value = "Education - College/University (campus-level)")] CollegeUniversity,
        [EnumMember(Value = "Education - General")] AdultEducation,
        [EnumMember(Value = "Education - K-12 School")] K12School,
        [EnumMember(Value = "Food Sales - Convenience Store (w/ or w/out gas station)")] ConvenienceStoreWithGasStation,
        [EnumMember(Value = "Food Sales - General")] WholesaleClubSupercenter,
        [EnumMember(Value = "Food Sales - Supermarket/Grocery")] SupermarketGroceryStore,
        [EnumMember(Value = "Food Service - Fast Food")] FastFoodRestaurant,
        [EnumMember(Value = "Food Service - General")] FoodService,
        [EnumMember(Value = "Food Service - Restaurant/Cafeteria")] Restaurant,
        [EnumMember(Value = "Health Care - Clinic")] UrgentCareClinicOtherOutpatient,
        [EnumMember(Value = "Health Care - Hospital Inpatient")] Hospital,
        [EnumMember(Value = "Health Care - Medical Office")] MedicalOffice,
        [EnumMember(Value = "Health Care - Nursing/Assisted Living")] SeniorCareCommunity,
        [EnumMember(Value = "Health Care - Outpatient - General")] OutpatientRehabilitationPhysicalTherapy,
        [EnumMember(Value = "Laboratory")] Laboratory,
        [EnumMember(Value = "Lodging - General")] otherLodgingResidential,
        [EnumMember(Value = "Lodging - Hotel/Motel")] Hotel,
        [EnumMember(Value = "Lodging - Residence Hall/Dormitory")] ResidenceHallDormitory,
        [EnumMember(Value = "Mixed-Use")] MixedUse,
        [EnumMember(Value = "Office - Small ( less than 10,000 sf)")] SmallOffice,
        [EnumMember(Value = "Office - Medium (10,000 to 100,000 sf)")] MediumOffice,
        [EnumMember(Value = "Office - Large ( greater than 100,000 sf)")] LargeOffice,
        [EnumMember(Value = "Other")] Other,
        [EnumMember(Value = "Public Assembly - Entertainment/Culture")] PerformingArts,
        [EnumMember(Value = "Public Assembly - General")] PublicAssembly,
        [EnumMember(Value = "Public Assembly - Library")] Library,
        [EnumMember(Value = "Public Assembly - Recreation")] Recreation,
        [EnumMember(Value = "Public Assembly - Social/Meeting")] SocialMeetingHall,
        [EnumMember(Value = "Public Safety - Fire/Police Station")] FireStation,
        [EnumMember(Value = "Public Safety - General")] otherPublicServices,
        [EnumMember(Value = "Religious Worship")] WorshipFacility,
        [EnumMember(Value = "Residential - Mid-Rise/High-Rise")] ResidentialHighRise,
        [EnumMember(Value = "Residential - Mobile Homes")] SingleFamilyHome,
        [EnumMember(Value = "Residential - Multi-Family, 2 to 4 units")] MultifamilyHousing,
        [EnumMember(Value = "Residential - Multi-Family, 5 or more units")] MultifamilyHousingLarge,
        [EnumMember(Value = "Residential - Single-Family Attached")] SingleFamilyHomeAttached,
        [EnumMember(Value = "Residential - Single-Family Detached")] SingleFamilyHomeDetached,
        [EnumMember(Value = "Retail - Mall")] RetailMall,
        [EnumMember(Value = "Retail - Non-mall, Vehicle Dealerships, misc.")] AutomobileDealership,
        [EnumMember(Value = "Retail Store")] RetailStore,
        [EnumMember(Value = "Service (vehicle repair/service, postal service)")] RepairServices,
        [EnumMember(Value = "Storage - Distribution/Shipping Center")] ShippingCenter,
        [EnumMember(Value = "Storage - General")] DistributionCenter,
        [EnumMember(Value = "Storage - Non-refrigerated warehouse")] Warehouse,
        [EnumMember(Value = "Storage - Refrigerated warehouse")] RefrigeratedWarehouse,
        [EnumMember(Value = "Warehouse - Self-storage")] SelfStorageFacility

    }







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

    [ProtoContract]
    public enum ScheduleCategory
    {
        [ProtoEnum]
        Control,
        [ProtoEnum] 
        Occupancy,
        [ProtoEnum]
        Availability, 
        [ProtoEnum] 
        Equipment, 
        [ProtoEnum] 
        Lights, 
        [ProtoEnum] 
        AirSpeed 
    };

    [ProtoContract]
    public enum HeatingCoolingModes
    {
        [ProtoEnum] 
        ConstantSetpoint,
        [ProtoEnum] 
        ScheduledSetpoint,
        [ProtoEnum] 
        Off 
    }


    [ProtoContract]
    public enum ConstructionCategory
    {
        [ProtoEnum]
        Facade, 
        [ProtoEnum]
        Roof,
        [ProtoEnum]
        GroundFloor,
        [ProtoEnum]
        InteriorFloor,
        [ProtoEnum]
        ExteriorFloor,
        [ProtoEnum]
        Partition
    };

    [ProtoContract]
    public enum GlazingConstructionTypes
    {
        [ProtoEnum]
        Other,
        [ProtoEnum]
        Single,
        [ProtoEnum]
        Double,
        [ProtoEnum]
        Triple,
        [ProtoEnum]
        Quadruple,
        [ProtoEnum]
        Generic
    };

    [ProtoContract]
    public enum OpaqueMaterialTypes
    {
        [ProtoEnum]
        Plaster,
        [ProtoEnum]
        Screed,
        [ProtoEnum]
        Concrete,
        [ProtoEnum]
        Boards,
        [ProtoEnum]
        Masonry, 
        [ProtoEnum]
        Insulation,
        [ProtoEnum]
        Timber,
        [ProtoEnum]
        Sealing,
        [ProtoEnum]
        Finishes,
        [ProtoEnum]
        Metal, 
        [ProtoEnum]
        Siding,
        [ProtoEnum]
        Other
    }






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
