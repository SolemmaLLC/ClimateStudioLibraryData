using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{

    [DataContract ]
    [ProtoContract]

    public class CSZoneConditioning : LibraryComponent
    {
        [DataMember, DefaultValue(1.0)]
        [ProtoMember(1)]
        public double CoolingCOP { get; set; } = 1;

        [DataMember, DefaultValue(1.0)]
        [ProtoMember(2)]
        public double HeatingCOP { get; set; } = 1;



        [DataMember, DefaultValue(20.0), Units("C")]
        [ProtoMember(3)]
        public double HeatingSetpoint { get; set; } = 20;


        [DataMember, DefaultValue(26.0), Units("C")]
        [ProtoMember(4)]
        public double CoolingSetpoint { get; set; } = 26;


        [DataMember, DefaultValue(true)]
        [ProtoMember(5)]
        public bool HeatingSetpointConstant { get; set; } = true;


        [DataMember, DefaultValue(true)]
        [ProtoMember(6)]
        public bool CoolingSetpointConstant { get; set; } = true;


        [DataMember, DefaultValue("S21 B18 8to18")]
        [ProtoMember(7)]
        public string HeatingSetpointSchedule { get; set; } = "S21 B18 8to18";


        [DataMember, DefaultValue("S25 B30 8to18")]
        [ProtoMember(8)]
        public string CoolingSetpointSchedule { get; set; } = "S25 B30 8to18";


        [DataMember, DefaultValue(true)]
        [ProtoMember(9)]
        public bool HeatIsOn { get; set; } = true;


        [DataMember, DefaultValue(true)]
        [ProtoMember(10)]
        public bool CoolIsOn { get; set; } = true;


        [DataMember, DefaultValue(false)]
        [ProtoMember(11)]
        public bool MechVentIsOn { get; set; } = false;


        [DataMember, DefaultValue(false)]
        [ProtoMember(12)]
        public bool HumidistatOnOff { get; set; } = false;




        [DataMember, DefaultValue(IdealSystemLimit.NoLimit)]
        [ProtoMember(13)]
        public IdealSystemLimit HeatingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";


        [DataMember, DefaultValue(IdealSystemLimit.NoLimit)]
        [ProtoMember(14)]
        public IdealSystemLimit CoolingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";


        [DataMember, DefaultValue(100.0)]
        [Units("W/m2")]
        [ProtoMember(15)]
        public double MaxHeatingCapacity { get; set; } = 100; //W/m2


        [DataMember, DefaultValue(100.0)]
        [Units("W/m2")]
        [ProtoMember(16)]
        public double MaxCoolingCapacity { get; set; } = 100;


        [DataMember, DefaultValue(100.0)]
        [Units("m3/s/m2")]
        [ProtoMember(17)]
        public double MaxHeatFlow { get; set; } = 100; //m3/s/m2


        [DataMember, DefaultValue(100.0)]
        [Units("m3/s/m2")]
        [ProtoMember(18)]
        public double MaxCoolFlow { get; set; } = 100;


        [DataMember, DefaultValue("AllOn")]
        [ProtoMember(19)]
        public string HeatingSchedule { get; set; } = "AllOn";


        [DataMember, DefaultValue("AllOn")]
        [ProtoMember(20)]
        public string CoolingSchedule { get; set; } = "AllOn";


        [DataMember, DefaultValue("AllOn")]
        [ProtoMember(21)]
        public string MechVentSchedule { get; set; } = "AllOn";


        [DataMember, DefaultValue(2.5)]
        [Units("L/s/p")]
        [ProtoMember(22)]
        public double MinFreshAirPerson { get; set; } = 2.5;


        [DataMember, DefaultValue(0.3)]
        [Units("L/s/m2")]
        [ProtoMember(23)]
        public double MinFreshAirArea { get; set; } = 0.3;


        [DataMember, DefaultValue(EconomizerItem.NoEconomizer)]
        [ProtoMember(24)]
        public EconomizerItem EconomizerType { get; set; } = EconomizerItem.NoEconomizer;//"NoEconomizer";


        [DataMember, DefaultValue(HeatRecoveryItem.None)]
        [ProtoMember(25)]
        public HeatRecoveryItem HeatRecoveryType { get; set; } = HeatRecoveryItem.None;// "None";


        [DataMember, DefaultValue(0.7)]
        [ProtoMember(26)]
        public double HeatRecoveryEfficiencySensible { get; set; } = 0.7;


        [DataMember, DefaultValue(0.65)]
        [ProtoMember(27)]
        public double HeatRecoveryEfficiencyLatent { get; set; } = 0.65;



        [DataMember, DefaultValue(20.0)]
        [Units("RH%")]
        [ProtoMember(28)]
        public double MinHumidity { get; set; } = 20;


        [DataMember, DefaultValue(80.0)]
        [Units("RH%")]
        [ProtoMember(29)]
        public double MaxHumidity { get; set; } = 80;



        [DataMember, DefaultValue(false)]
        [ProtoMember(30)]
        public bool EMSFanEnergyIsOn { get; set; } = false;


        [DataMember, DefaultValue(1000.0)]
        [Units("Pa")]
        [ProtoMember(31)]
        public double FanPressureRise { get; set; } = 1000;






        public CSZoneConditioning()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
