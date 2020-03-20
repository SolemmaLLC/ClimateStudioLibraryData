using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract ]
    [ProtoContract]

    public class CSZoneVentilation : LibraryComponent
    {
        [DataMember, DefaultValue(false)]
        [ProtoMember(1)]
        public bool NatVentIsOn { get; set; } = false;


        [DataMember, DefaultValue(false)]
        [ProtoMember(2)]
        public bool SchedVentIsOn { get; set; } = false;


        [DataMember, DefaultValue(true)]
        [ProtoMember(3)] 
        public bool InfiltrationIsOn { get; set; } = true;


 
        [DataMember,DefaultValue(1.0)]
        [ProtoMember(4)]
        public double InfiltrationConstantCoefficient { get; set; } =1.0;


        [DataMember,DefaultValue(0.0)]
        [ProtoMember(5)]
        public double InfiltrationTemperatureCoefficient { get; set; } = 0.0;


        [DataMember,DefaultValue(0.0)]
        [ProtoMember(6)]
        public double InfiltrationWindVelocityCoefficient { get; set; } = 0.0;


        [DataMember,DefaultValue(0.0)]
        [ProtoMember(7)]
        public double InfiltrationWindVelocitySquaredCoefficient { get; set; } = 0.0;





        [DataMember, DefaultValue(0.1), Units("ACH")]
        [ProtoMember(8)]
        public double InfiltrationAch { get; set; } = 0.1;


        [DataMember, DefaultValue(0.6), Units("ACH")]
        [ProtoMember(9)]
        public double ScheduledVentilationAch { get; set; } = 0.6;



        [DataMember, DefaultValue("AllOn")]
        [ProtoMember(10)]
        public string ScheduledVentilationSchedule { get; set; } = "AllOn";


        [DataMember , DefaultValue("AllOn")]
        [ProtoMember(11)]
        public string NatVentSchedule { get; set; } = "AllOn";


        [DataMember, DefaultValue(18.0), Units("C")]
        [ProtoMember(12)]
        public double ScheduledVentilationSetPoint { get; set; } = 18;


        [DataMember, DefaultValue(true)]
        [ProtoMember(13)]
        public bool BuoyancyDrivenIsOn { get; set; } = true;


        [DataMember, DefaultValue(false)]
        [ProtoMember(14)]
        public bool WindDrivenIsOn { get; set; } = false;


        [DataMember, DefaultValue(22.0),Units("C")]
        [ProtoMember(15)]
        public double NatVentSetPoint { get; set; } = 22;


        [DataMember, DefaultValue(0.0),Units("C")]
        [ProtoMember(16)]
        public double NatVentMinOutAirTemp { get; set; } = 0;


        [DataMember, DefaultValue(30.0), Units("C")]
        [ProtoMember(17)]
        public double NatVentMaxOutAirTemp { get; set; } = 30;


        [DataMember, DefaultValue(90.0), Units("RH%")]
        [ProtoMember(18)]
        public double NatVentMaxRelHum { get; set; } = 90;



        [DataMember]
        [DefaultValue(0.001)]
        [ProtoMember(19)]
        public double AFN_AirMassFlowCoefficient_Crack { get; set; } = 0.001;

        public CSZoneVentilation()
        {
        }

        public override string ToString() { return Serialization.Serialize(this); }
    }
}
