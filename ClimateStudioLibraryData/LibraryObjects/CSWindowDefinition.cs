using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract ]
    [ProtoContract]
    public class CSWindowDefinition : LibraryComponent
    {
        public CSWindowDefinition() { }
        public override string ToString() { return this.toJSON(); }
        public bool isValid()
        {

            var props = typeof(CSWindowDefinition).GetProperties();

            foreach (var prop in props)
            {

                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name.ToString() + " IS NULL");
            }

            return true;
        }




        [DataMember]
        [ProtoMember(1)]
        public windowType Type { get; set; } = windowType.External;
        [DataMember]
        [ProtoMember(2)]
        public bool ShadingSystemIsOn { get; set; } = false;
        [DataMember]
        [ProtoMember(3)]
         public ShadingType ShadingSystemType { get; set; } = ShadingType.ExteriorShade;//"ExteriorShade";
        [DataMember]
        [ProtoMember(4)]
        public ShadingControlType ShadingControlType { get; set; } = ShadingControlType.OnIfHighSolarOnWindow;
        [DataMember]
        [ProtoMember(5)]
        public string ShadingSystemAvailibilitySchedule { get; set; } = "AllOn";
        [DataMember]
        [Units("W/m2")]
        [ProtoMember(6)]
        public double ShadingSystemSetPoint { get; set; } = 180;
        [DataMember]
        [ProtoMember(7)]
        public double ShadingSystemTransmittance { get; set; } = 0.5;



        [DataMember]
        [Units("W/m-K")]
        [ProtoMember(8)]
        public double ShadingConductivity { get; set; } = 0.1;
        [DataMember]
        [Units("m")]
        [ProtoMember(9)]
        public double ShadingThickness { get; set; } = 0.003;
        [DataMember]
        [Units("m")]
        [ProtoMember(10)]
        public double ShadeGlassDistance { get; set; } = 0.05;
        [DataMember]
        [ProtoMember(11)]
        public double TopBottomEdgeOpeningMultiplier { get; set; } = 1;



        [DataMember]
        [ProtoMember(12)]
        public bool IsVirtualPartition { get; set; } = false;
        [DataMember]
        [ProtoMember(13)]
        public bool ZoneMixingIsOn { get; set; } = false;
        [DataMember]
        [ProtoMember(14)]
        public double ZoneMixingDeltaTemperature { get; set; } = 2.0;
        [DataMember]
        [Units("m3/s")]
        [ProtoMember(15)]
        public double ZoneMixingFlowRate { get; set; } = 0.001; // Volumenstrom m3/s  == 1 l/s

        [DataMember]
        [ProtoMember(16)]
        public string Construction { get; set; } = "defaultGlazing";






        [DataMember]
        [ProtoMember(17)]
        public double OperableArea { get; set; } = 0.8;
   
        [DataMember]
        [ProtoMember(18)]
        public string ZoneMixingAvailibilitySchedule { get; set; } = "AllOn";


        //Frame
        [DataMember]
        [ProtoMember(19)]
        public bool HasFrame = false;

        [DataMember, Units("m")]
        [ProtoMember(20)]
        public double FrameWidth { get; set; } = 0.05;
        [DataMember, Units("m")]
        [ProtoMember(21)]
        public double FrameProjection { get; set; } = 0.01;
        [DataMember, Units("W/m2-K")]
        [ProtoMember(22)]
        public double FrameConductance { get; set; } = 5.0;


        [DataMember, Units("m")]
        [ProtoMember(23)]
        public double DividerWidth { get; set; } = 0.025;

        [DataMember, Units("m")]
        [ProtoMember(24)]
        public double InsideSillRevealDepth { get; set; } = 0.2;




        //AFN

        [DataMember]
        [Units("C")]
        [ProtoMember(25)]
        public double AFN_TEMP_SETPT { get; set; } = 20;

        [DataMember]
        [ProtoMember(26)]
        public string AFN_WIN_AVAIL { get; set; } = "AllOn";

        [DataMember]
        [ProtoMember(27)]
        public double AFN_DISCHARGE_C { get; set; } = 0.65;


        [DataMember]
        [ProtoMember(28)]
        public AFNVentilationControl VentControl { get; set; } = AFNVentilationControl.Temperature;


        [DataMember]
        [ProtoMember(29)]
        public List<double> CPArray { get; set; } = new List<double>();










        public CSLibrary getAllLibraryReferences(CSLibrary library)
        {
            var newLib = new CSLibrary();

           

            if (library.GlazingConstructions.Any(o => o.Name == this.Construction))
            {
                var ys = library.GlazingConstructions.First(o => o.Name == this.Construction);
                newLib.Add(ys);
            }
            else if (library.GlazingConstructionsSimple.Any(o => o.Name == this.Construction))
            {
                newLib.Add(library.GlazingConstructionsSimple.First(o => o.Name == this.Construction));
            }


            HashSet<string> List_usedYearSchedules = new HashSet<string>();


            // add zone schedules
            List_usedYearSchedules.Add(this.ShadingSystemAvailibilitySchedule);
            List_usedYearSchedules.Add(this.ZoneMixingAvailibilitySchedule);
            List_usedYearSchedules.Add(this.AFN_WIN_AVAIL);
        


            foreach (string s in List_usedYearSchedules.ToList())
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    continue;
                }
                // check if year schedule is not array schedule 
                if (library.YearSchedules.Any(o => o.Name == s))
                {
                    var ys = library.YearSchedules.First(o => o.Name == s);
                    newLib.Add(ys);
                }
                else if (library.ArraySchedules.Any(o => o.Name == s))
                {
                    newLib.Add(library.ArraySchedules.First(o => o.Name == s));
                }
            }

            return newLib;
        }












        public static CSWindowDefinition HardCopy(CSWindowDefinition zsc)
        {
            string s = zsc.toJSON();
            return CSWindowDefinition.fromJSON(s);
        }




        public static CSWindowDefinition fromJSON(string json)
        {
            return Serialization.Deserialize<CSWindowDefinition>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSWindowDefinition>(this);
        }


        public string buffMe()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, this);
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
        public static CSWindowDefinition unBuffMe(string txt)
        {
            byte[] arr = Convert.FromBase64String(txt);
            using (MemoryStream ms = new MemoryStream(arr))
                return ProtoBuf.Serializer.Deserialize<CSWindowDefinition>(ms);
        }
    }

}
