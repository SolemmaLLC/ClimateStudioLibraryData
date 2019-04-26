using ArchsimLib.Utilities;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class HVACZoneVAV : LibraryComponent
    {
        [DataMember]
        public double FanTotalEfficiency { get; set; } = 0.7;
        [DataMember]
        public double FanDeltaPressure { get; set; } = 600;
        [DataMember]
        public double FanMotorEfficiency { get; set; } = 0.9;


        [DataMember]
        public double HeatingSetpoint { get; set; } = 20;//needed for trnsys 
        [DataMember]
        public double CoolingSetpoint { get; set; } = 26;//needed for trnsys

        [Units("L/s/p")]
        public double MinFreshAirPerson { get; set; } = 2.5;
        [DataMember]
        [Units("L/s/m2")]
        public double MinFreshAirArea { get; set; } = 0.3;






        //[DataMember]
        //public string HeatingSetpointSchedule { get; set; }= "AllOn";
        //[DataMember]
        //public string CoolingSetpointSchedule { get; set; }= "AllOn";


        //[DataMember]
        //public bool HeatIsOn { get; set; } = true;
        //[DataMember]
        //public bool CoolIsOn { get; set; } = true;
        //[DataMember]
        //public bool MechVentIsOn { get; set; } = false;
        //[DataMember]
        //public bool HumidistatOnOff { get; set; } = false;


        //[DataMember]
        ////[JsonConverter(typeof(StringEnumConverter))]
        //public IdealSystemLimit HeatingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";
        //[DataMember]
        ////[JsonConverter(typeof(StringEnumConverter))]
        //public IdealSystemLimit CoolingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";
        //[DataMember]
        //[Units("W/m2")]
        //public double MaxHeatingCapacity { get; set; } = 100; //W/m2
        //[DataMember]
        //[Units("W/m2")]
        //public double MaxCoolingCapacity { get; set; } = 100;
        //[DataMember]
        //[Units("m3/s/m2")]
        //public double MaxHeatFlow { get; set; } = 100; //m3/s/m2
        //[DataMember]
        //[Units("m3/s/m2")]
        //public double MaxCoolFlow { get; set; } = 100;
        //[DataMember]
        //public string HeatingSchedule { get; set; } = "AllOn";
        //[DataMember]
        //public string CoolingSchedule { get; set; } = "AllOn";



        //[DataMember]
        //public string MechVentSchedule { get; set; } = "AllOn";
        //[DataMember]

        //[DataMember]
        ////[JsonConverter(typeof(StringEnumConverter))]
        //public EconomizerItem EconomizerType { get; set; } = EconomizerItem.NoEconomizer;//"NoEconomizer";
        //[DataMember]
        ////[JsonConverter(typeof(StringEnumConverter))]
        //public HeatRecoveryItem HeatRecoveryType { get; set; } = HeatRecoveryItem.None;// "None";
        //[DataMember]
        //public double HeatRecoveryEfficiencySensible { get; set; } = 0.7;
        //[DataMember]
        //public double HeatRecoveryEfficiencyLatent { get; set; } = 0.65;



        //[DataMember]
        //[Units("RH%")]
        //public double MinHumidity { get; set; } = 20;


        public HVACZoneVAV()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }


    [DataContract(IsReference = true)]
    public class HVACSystemVAV : LibraryComponent
    {
        [DataMember]
        public double ChillerCOP { get; set; } = 3.2;

        [DataMember]
        public double BoilerEFF { get; set; } = 0.8;


        [DataMember]
        public string SystemAvailabilitySchedule { get; set; } = "AllOn";
        

        [DataMember]
        public double HeatingCoilSetpoint { get; set; } = 10.0;
        [DataMember]
        public double CoolingCoilSetpoint { get; set; } = 12.8;


        [DataMember]
        [Units("RH%")]
        public double HumidifierSetpoint { get; set; } = 30;
        [DataMember]
        [Units("RH%")]
        public double DehumidificationSetpoint { get; set; } = 80;



        [DataMember]
        public double SupplyFanTotalEfficiency { get; set; } = 0.7;
        [DataMember]
        public double SupplyFanDeltaPressure { get; set; } = 600;
        [DataMember]
        public double SupplyFanMotorEfficiency { get; set; } = 0.9;


        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public EconomizerItemHVACTemp EconomizerType { get; set; } = EconomizerItemHVACTemp.NoEconomizer;//"NoEconomizer";
        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public HeatRecoveryItem HeatRecoveryType { get; set; } = HeatRecoveryItem.None;// "None";
        [DataMember]
        public double HeatRecoveryEfficiencySensible { get; set; } = 0.7;
        [DataMember]
        public double HeatRecoveryEfficiencyLatent { get; set; } = 0.65;


        public DehumidificationControlType DehumidificationControlType { get; set; } = DehumidificationControlType.None;
        public HumidifierType HumidifierType { get; set; } = HumidifierType.None;


        public FanPartLoadPowerCoefficients FanPartLoadPowerCoefficients { get; set; } = FanPartLoadPowerCoefficients.InletVaneDampers;


        public HVACSystemVAV()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
