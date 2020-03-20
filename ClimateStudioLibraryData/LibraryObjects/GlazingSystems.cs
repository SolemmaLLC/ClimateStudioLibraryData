using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSEnergyLib.LibraryObjects
{
    #region GlazingConstruction


    [DataContract]
    [ProtoContract]
    public class CSGlazingConstruction : CSBaseConstruction
    {
        // not used in simulation begin
        [DataMember]
        [ProtoMember(1)]
        public double SHGF { get; set; } = 0;


        [DataMember]
        [Units("W/m2-k")]
        [ProtoMember(2)]
        public double UValue { get; set; } = 0;


        [DataMember]
        [ProtoMember(3)]
        public double TVis { get; set; } = 0;
        // not used in simulation end

        // optical properties
        [DataMember]
        [ProtoMember(4)]

        public float T_R { get; set; } = 0; // transmission color
        [DataMember]
        [ProtoMember(5)]

        public float T_G { get; set; } = 0;
        [DataMember]
        [ProtoMember(6)]

        public float T_B { get; set; } = 0;
        [DataMember]
        [ProtoMember(7)]

        public float Rf_R { get; set; } = 0; // front reflection color
        [DataMember]
        [ProtoMember(8)]

        public float Rf_G { get; set; } = 0;
        [DataMember]
        [ProtoMember(9)]

        public float Rf_B { get; set; } = 0;
        [DataMember]
        [ProtoMember(10)]

        public float Rb_R { get; set; } = 0; // back reflection color
        [DataMember]
        [ProtoMember(11)]

        public float Rb_G { get; set; } = 0;
        [DataMember]
        [ProtoMember(12)]

        public float Rb_B { get; set; } = 0;
        [DataMember]
        [ProtoMember(13)]

        public float TSol { get; set; } = 0; // solar
        [DataMember]
        [ProtoMember(14)]

        public float Rf_Sol { get; set; } = 0; 
        [DataMember]
        [ProtoMember(15)]

        public float Rb_Sol { get; set; } = 0;
        [DataMember]
        [ProtoMember(16)]

        public string BSDF_FilePath { get; set; } // optional
        [DataMember]
        [ProtoMember(17)]

        public string Func_FilePath { get; set; } // other functions (optional)

        [DataMember]
        [ProtoMember(18)]

        public GlazingConstructionTypes Type { get; set; } = GlazingConstructionTypes.Single;

        [DataMember]
        [ProtoMember(19)]

        public List<CSWindowMaterialBase> Layers { get; set; } = new List<CSWindowMaterialBase>();
        
        
        
        public CSGlazingConstruction() { }
        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

          //  foreach (var l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        public static string GetRadianceMaterial(CSGlazingConstruction c)
        {
            //return "void plastic White_50 0 0 5 0.5 0.5 0.5 0 0";
            //return "void glass glass_70 0 0 3 0.77 0.77 0.77";

            double trans = 1.0;

            foreach (var l in c.Layers)
            {
                CSGlazingMaterial gl = l as CSGlazingMaterial;
                if (gl != null) trans *= gl.VisibleTransmittance;
            }

            double refractiveIndex = 1.52;
            double rad = (Math.Sqrt(0.8402528435 + 0.0072522239 * trans * trans) - 0.9166530661) / 0.0036261119 / trans;
            return String.Format("void glass {2}_glazing 0 0 4 {0} {0} {0} {1}\n", rad, refractiveIndex, c.Name);
        }
        public string GetInfo()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("UValue[W/m2K] = unknown");// + Math.Round(this.UValue, 3));
            s.AppendLine("SHGC = unknown");// + Math.Round(this.SHGF, 3));
            s.AppendLine("Tvis = unknown");// + Math.Round(this.VisibleTransmittance, 3));

            foreach (var l in this.Layers)
            {

                s.AppendLine(l + " " + l.Thickness + " [m]");
            }
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            // s.AppendLine("Cost[$/m2] = " + this.GetDollar());

            return s.ToString();
        }
        public string GetIndicators()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("U-Value[W/m2K] = " + Math.Round(this.UValue, 3));
            s.AppendLine("SHGC = " + Math.Round(this.SHGF, 3));
            s.AppendLine("TVIS = " + Math.Round(this.TVis, 3));
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            return s.ToString();
        }
        public string GetLayers()
        {
            StringBuilder s = new StringBuilder();

            int cnt = 1;
            foreach (var l in this.Layers)
            {
                string prefix = " " + cnt + " - ";
                //if(cnt ==1 ) prefix = " O - ";
                //else if (cnt == this.Layers.Count) prefix = " I - ";
                s.AppendLine(prefix + l.Name + " " + Math.Round(l.Thickness* 1000) + " [mm]");
                cnt++;
            }


            return s.ToString();
        }
        public double GetDollar()
        {
            double dollar = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    dollar += lay.Thickness * mat.Cost;
                }
            }
            return dollar + this.Cost;
        }
        public double GetEE()
        {
            double ee = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    ee += lay.Thickness * mat.Density *
                          mat.EmbodiedEnergy;
                }
            }
            return ee + this.EmbodiedEnergy;
        }
        public double GetEC()
        {
            double ec = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    ec += lay.Thickness * mat.Density *
                          mat.EmbodiedCarbon;
                }
            }
            return ec + this.EmbodiedCarbon;
        }
        public double[] GetDollar(int LifeTime, double area)
        {
            double[] dollar = new double[LifeTime];
            foreach (var lay in this.Layers)
            {

                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    var dd = lay.Thickness * mat.Cost;

                    for (int lft = 0; lft < LifeTime; lft += mat.Life)
                    {
                        dollar[lft] += dd * area;
                    }
                }
            }
            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                dollar[lft] += this.Cost * area;
            }
            return dollar;
        }
        public double[] GetEE(int LifeTime, double area)
        {
            double[] ee = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    var dd = lay.Thickness * mat.Density * mat.EmbodiedEnergy;

                    for (int lft = 0; lft < LifeTime; lft += mat.Life)
                    {
                        ee[lft] += dd * area;
                    }
                }
            }
            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ee[lft] += this.EmbodiedEnergy * area;
            }
            return ee;
        }
        public double[] GetEC(int LifeTime, double area)
        {
            double[] ec = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                var mat = lay as CSGlazingMaterial;
                if (mat != null)
                {
                    var dd = lay.Thickness * mat.Density * mat.EmbodiedCarbon;

                    for (int lft = 0; lft < LifeTime; lft += mat.Life)
                    {
                        ec[lft] += dd * area;
                    }
                }
            }

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ec[lft] += this.EmbodiedCarbon * area;
            }

            return ec;
        }

        public static CSGlazingConstruction fromJSON(string json)
        {
            return Serialization.Deserialize<CSGlazingConstruction>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSGlazingConstruction>(this);
        }
    }


    #endregion


    #region GlazingConstructionSimple




    [DataContract]
    [ProtoContract]
    public class CSGlazingConstructionSimple : CSBaseConstruction 
    {

        [DataMember]
        [ProtoMember(1)]
        public double SHGF { get; set; } = 0.837;
        [DataMember]
        [Units("W/m2-k")]
        [ProtoMember(2)]
        public double UValue { get; set; } = 0.075;
        [DataMember]
        [ProtoMember(3)]
        public double VisibleTransmittance { get; set; } = 0.898;

        [DataMember]
        [ProtoMember(4)]
        public GlazingConstructionTypes Type { get; set; } = GlazingConstructionTypes.Single;

        public CSGlazingConstructionSimple() { }
        public CSGlazingConstructionSimple(string name, string category, string comment, double tvis, double uval, double shgf)
        {

            this.Name = name.Trim();
            this.Category = category.Trim();
            this.Comment = comment.Trim();
            this.VisibleTransmittance = tvis;
            this.UValue = uval;
            this.SHGF = shgf;
            this.Comment = comment;
        }

        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            if (this.SHGF < 0.0) { this.SHGF = 0.0; changed = true; }
            if (this.SHGF > 1.0) { this.SHGF = 1.0; changed = true; }

            if (this.VisibleTransmittance < 0.0) { this.VisibleTransmittance = 0.0; changed = true; }
            if (this.VisibleTransmittance > 1.0) { this.VisibleTransmittance = 1.0; changed = true; }

            return changed;
        }


        //Simple Glazing constructions

        public string GetInfo()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("UValue[W/m2K] = " + Math.Round(this.UValue, 3));
            s.AppendLine("SHGC = " + Math.Round(this.SHGF, 3));
            s.AppendLine("Tvis = " + Math.Round(this.VisibleTransmittance, 3));

            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            // s.AppendLine("Cost[$/m2] = " + this.GetDollar());

            return s.ToString();
        }

        public string GetIndicators()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("U-Value[W/m2K] = " + Math.Round(this.UValue, 3));
            s.AppendLine("SHGC = " + Math.Round(this.SHGF, 3));
            s.AppendLine("TVIS = " + Math.Round(this.VisibleTransmittance, 3));
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            return s.ToString();
        }

        public string GetLayers()
        {
             
            return "Simple Glazing Model";
        }

        public string GetRadianceMaterial()
        {
            double trans = this.VisibleTransmittance;

            double refractiveIndex = 1.52;
            double rad = (Math.Sqrt(0.8402528435 + 0.0072522239 * trans * trans) - 0.9166530661) / 0.0036261119 / trans;
            return String.Format("void glass {2}_glazing 0 0 4 {0} {0} {0} {1}\n", rad, refractiveIndex, this.Name);
        }
        public double GetDollar()
        {
            return this.Cost;
        }
        public double GetEE()
        {

            return this.EmbodiedEnergy;
        }
        public double GetEC()
        {

            return this.EmbodiedCarbon;
        }
        public double[] GetDollar(int LifeTime, double area)
        {
            double[] dollar = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                dollar[lft] += this.Cost * area;
            }

            return dollar;
        }
        public double[] GetEE(int LifeTime, double area)
        {
            double[] ee = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ee[lft] += this.EmbodiedEnergy * area;
            }

            return ee;
        }
        public double[] GetEC(int LifeTime, double area)
        {
            double[] ec = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ec[lft] += this.EmbodiedCarbon * area;
            }

            return ec;
        }


        public static CSGlazingConstructionSimple fromJSON(string json)
        {
            return Serialization.Deserialize<CSGlazingConstructionSimple>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSGlazingConstructionSimple>(this);
        }
    }


    #endregion
}
