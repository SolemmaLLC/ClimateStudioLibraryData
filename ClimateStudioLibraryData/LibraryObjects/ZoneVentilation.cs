using ArchsimLib.Utilities;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class ZoneVentilation : LibraryComponent
    {
        [DataMember]
        public bool NatVentIsOn { get; set; } = false;
        [DataMember]
        public bool SchedVentIsOn { get; set; } = false;
        [DataMember]
        public bool InfiltrationIsOn { get; set; } = true;


        //[DataMember]
        //[DefaultValue(InfiltrationModel.Constant)]
        //public InfiltrationModel InfiltrationModel { get; set; } = InfiltrationModel.Constant;
        [DataMember]
        [DefaultValue(1.0)]
        public double InfiltrationConstantCoefficient { get; set; } =1.0;
        [DataMember]
        [DefaultValue(0.0)]
        public double InfiltrationTemperatureCoefficient { get; set; } = 0.0;
        [DataMember]
        [DefaultValue(0.0)]
        public double InfiltrationWindVelocityCoefficient { get; set; } = 0.0;
        [DataMember]
        [DefaultValue(0.0)]
        public double InfiltrationWindVelocitySquaredCoefficient { get; set; } = 0.0;



        [DataMember]
        [Units("ACH")]
        public double InfiltrationAch { get; set; } = 0.1;
        [DataMember]
        [Units("ACH")]
        public double ScheduledVentilationAch { get; set; } = 0.6;



        [DataMember]
        [DefaultValue("AllOn")]
        public string ScheduledVentilationSchedule { get; set; } = "AllOn";
        [DataMember]
        [DefaultValue("AllOn")]
        public string NatVentSchedule { get; set; } = "AllOn";
        [DataMember]
        [Units("C")]
        public double ScheduledVentilationSetPoint { get; set; } = 18;
        [DataMember]
        public bool BuoyancyDrivenIsOn { get; set; } = true;
        [DataMember]
        public bool WindDrivenIsOn { get; set; } = false;
        [DataMember]
        [Units("C")]
        public double NatVentSetPoint { get; set; } = 18;
        [DataMember]
        [Units("C")]
        public double NatVentMinOutAirTemp { get; set; } = 0;
        [DataMember]
        [Units("C")]
        public double NatVentMaxOutAirTemp { get; set; } = 30;
        [DataMember]
        [Units("RH%")]
        public double NatVentMaxRelHum { get; set; } = 90;
        //[DataMember]
        //public bool AFN { get; set; } = false;

        public ZoneVentilation()
        {
        }

        public override string ToString() { return Serialization.Serialize(this); }
    }
}
