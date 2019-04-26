using ArchsimLib.LibraryObjects;
using ArchsimLib.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArchsimLib.LibraryObjects
{

    #region OpaqueConstruction

    [DataContract]
    public class OpaqueConstruction : BaseConstruction
    {
        [DataMember]
        public List<Layer<OpaqueMaterial>> Layers = new List<Layer<OpaqueMaterial>>();

        [DataMember]
        public ConstructionCategory Type { get; set; } = ConstructionCategory.Facade;

        public OpaqueConstruction() { }

        public bool Correct()
        {
            bool changed = false;

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (Layer<OpaqueMaterial> l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        public string GetInfo() {

            StringBuilder s = new StringBuilder();
            s.AppendLine("UValue[W/m2K] = " + Math.Round(this.GetUval(), 3));
            s.AppendLine("HeatCapacity[kJ/m2K] = " + Math.Round(this.GetHeatCapacity() , 3));

            foreach (var l in this.Layers)
            {
                s.AppendLine(l.Material + " " + l.Thickness + " [m]");
            }
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            // s.AppendLine("Cost[$/m2] = " + this.GetDollar());

            return s.ToString();
        }

        private double GetRvalueLayers()
        {
            double rval = 0.0;

            foreach (var lay in this.Layers)
            {
                double cond = lay.Material.Conductivity;
                double rn = lay.Thickness / cond;
                rval += rn;
            }

            return rval;
        }
        public  double GetRvalue()
        {

            ConstructionCategory tp = this.Type;

            double rval = this.GetRvalueLayers();

            double ro = 0.0;
            double ri = 0.0;
            if (tp == ConstructionCategory.Facade) { ro = 0.04; ri = 0.13; }
            else if (tp == ConstructionCategory.Roof) { ro = 0.04; ri = 0.10; }
            else if (tp == ConstructionCategory.ExteriorFloor) { ro = 0.04; ri = 0.17; }
            else if (tp == ConstructionCategory.InteriorFloor) { ro = 0.17; ri = 0.17; }
            else { ro = 0.13; ri = 0.13; }

            rval += ro;
            rval += ri;

            return rval;
        }
        public  double GetUval()
        {
            return 1.0 / this.GetRvalue();
        }
        /// <summary>
        /// Units [kJ/m2K]
        /// </summary>
        /// <returns></returns>
        public double GetHeatCapacity()
        {
            double hc = 0.0;

            foreach (var lay in this.Layers)
            {
                hc += lay.Material.SpecificHeat * lay.Material.Density * lay.Thickness;
            }

            return hc / 1000.0;
        }
        public  string GetRadianceMaterial()
        {
            //return "void plastic White_50 0 0 5 0.5 0.5 0.5 0 0";
            //return "void glass glass_70 0 0 3 0.77 0.77 0.77";

            const double LuminousEfficacyRed = 0.3;
            const double LuminousEfficacyGreen = 0.59;
            const double LuminousEfficacyBlue = 0.11;

            double Refl = 1.0 - this.Layers.Last().Material.VisibleAbsorptance;

            double Red = 255;
            double Green = 255;
            double Blue = 255;
            double Specularilty = 0;
            double Roughness = 0;

            double w = Red * LuminousEfficacyRed + Green * LuminousEfficacyGreen + Blue * LuminousEfficacyBlue;

            return String.Format("void plastic {5}_opaque 0 0 5 {0} {1} {2} {3} {4}\n", (Red / w * Refl), (Green / w * Refl), (Blue / w * Refl), Specularilty, Roughness, this.Name);
        }
        public  double GetDollar()
        {
            double dollar = 0.0;
            foreach (var lay in this.Layers)
            {
                dollar += lay.Thickness * lay.Material.Cost;
            }
            dollar += this.Cost;
            return dollar + this.Cost;
        }
        public  double GetEE()
        {
            double ee = 0.0;
            foreach (var lay in this.Layers)
            {
                ee += lay.Thickness * lay.Material.Density * lay.Material.EmbodiedEnergy;
            }
            ee += this.EmbodiedEnergy;
            return ee + this.EmbodiedEnergy;
        }
        public  double GetEC()
        {
            double ec = 0.0;
            foreach (var lay in this.Layers)
            {
                ec += lay.Thickness * lay.Material.Density * lay.Material.EmbodiedCarbon;
            }
            ec += this.EmbodiedCarbon;
            return ec + this.EmbodiedCarbon;
        }
        public  double[] GetDollar( int LifeTime, double area)
        {
            double[] dollar = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                // if (lay.GasType == "GasMaterial") continue;
                OpaqueMaterial glmat = lay.Material;
                var dd = lay.Thickness * glmat.Cost;

                //dd += c.Cost;

                for (int lft = 0; lft < LifeTime; lft += glmat.Life)
                {
                    dollar[lft] += dd * area;
                }
            }
            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                dollar[lft] += this.Cost * area;
            }
            return dollar;
        }
        public  double[] GetEE( int LifeTime, double area)
        {
            double[] ee = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                //if (lay.GasType == "GasMaterial") continue;
                OpaqueMaterial glmat = lay.Material;
                var dd = lay.Thickness * glmat.Density * glmat.EmbodiedEnergy;

                //dd += c.EmbodiedEnergy;

                for (int lft = 0; lft < LifeTime; lft += glmat.Life)
                {
                    ee[lft] += dd * area;
                }
            }
            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ee[lft] += this.EmbodiedEnergy * area;
            }
            return ee;
        }
        public  double[] GetEC(int LifeTime, double area)
        {
            double[] ec = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                //  if (lay.GasType == "GasMaterial") continue;
                OpaqueMaterial glmat = lay.Material;
                var dd = lay.Thickness * glmat.Density * glmat.EmbodiedCarbon;

                // dd += c.EmbodiedCarbon;

                for (int lft = 0; lft < LifeTime; lft += glmat.Life)
                {
                    ec[lft] += dd * area;
                }
            }
            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ec[lft] += this.EmbodiedCarbon * area;
            }
            return ec;
        }

        public static OpaqueConstruction QuickConstruction(string name, ConstructionCategory type, string[] layers, double[] thickness, string category, string source, ref Library Library)
        {

            OpaqueConstruction oc = new OpaqueConstruction();
            for (int i = 0; i < layers.Length; i++)
            {
                try
                {
                    if (thickness.Length != layers.Length) { continue; }
                    if (!(thickness[i] > 0)) { continue; }

                    if (Library.OpaqueMaterials.Any(x => x.Name == layers[i]))
                    {

                        var mat = Library.OpaqueMaterials.First(o => o.Name == layers[i]);
                        Layer<OpaqueMaterial> layer = new Layer<OpaqueMaterial>(thickness[i], mat);
                        oc.Layers.Add(layer);
                    }
                    else
                    {

                        Debug.WriteLine("ERROR: " + "Could not find " + layers[i]);
                        Logger.WriteLine("ERROR: " + "Could not find " + layers[i]);
                        return null;

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            }

            oc.Name = name;
            oc.Type = type;
            oc.Category = category;
            oc.DataSource = source;


            Library.Add(oc);
            return oc;

        }
        public static OpaqueConstruction MixConstructions(string newName, OpaqueConstruction c1, OpaqueConstruction c2, double areaFractionC2)
        {

            //find layer with lowest conductivity in C1
            double minLamdaC1 = double.MaxValue;
            double thicknessMinLamdaC1 = 0;
            int minLamdaC1Index = -1;
            double thicknessC1 = 0;

            for (int i = 0; i < c1.Layers.Count; i++)
            {
                var layer = c1.Layers[i];
                thicknessC1 += layer.Thickness;
                if (layer.Material.Conductivity < minLamdaC1)
                {
                    minLamdaC1 = layer.Material.Conductivity;
                    thicknessMinLamdaC1 = layer.Thickness;
                    minLamdaC1Index = i;
                }
            }

            double rLayer = thicknessMinLamdaC1 / minLamdaC1;

            // claculate target R-value
            double r1 = c1.GetRvalueLayers();
            double r2 = c2.GetRvalueLayers();
            double rTarget = r1 * (1.0 - areaFractionC2) + r2 * areaFractionC2;
            double r1_2 = r1 - rLayer;
            double newThickness = (rTarget - r1_2) * minLamdaC1;


            List<Layer<OpaqueMaterial>> newLayerset = new List<Layer<OpaqueMaterial>>();
            for (int i = 0; i < c1.Layers.Count; i++)
            {
                if (i == minLamdaC1Index) newLayerset.Add(new Layer<OpaqueMaterial>(newThickness, c1.Layers[i].Material));
                else newLayerset.Add(new Layer<OpaqueMaterial>(c1.Layers[i].Thickness, c1.Layers[i].Material));
            }

            OpaqueConstruction c = new OpaqueConstruction()
            {
                Name = newName,
                Type = c1.Type,
                EmbodiedCarbon = c1.EmbodiedCarbon * (1.0 - areaFractionC2) + c2.EmbodiedCarbon * areaFractionC2,
                Cost = c1.Cost * (1.0 - areaFractionC2) + c2.Cost * areaFractionC2,
                EmbodiedEnergy = c1.EmbodiedEnergy * (1.0 - areaFractionC2) + c2.EmbodiedEnergy * areaFractionC2,
                Life = c1.Life,
                Comment = c1.Comment,
                Category = c1.Category,
                DataSource = c1.DataSource,
                Layers = newLayerset
            };
            return c;
        }

    }

    #endregion


    #region GlazingConstruction


    [DataContract]
    public class GlazingConstruction : BaseConstruction
    {
        // not used in simulation begin
        [DataMember]
        public string SHGF { get; set; } = "?";
        [DataMember]
        [Units("W/m2-k")]
        public string UValue { get; set; } = "?";
        [DataMember]
        public string TVis { get; set; } = "?";
        // not used in simulation end

        [DataMember]
        public List<Layer<WindowMaterialBase>> Layers = new List<Layer<WindowMaterialBase>>();
        public GlazingConstruction() { }
        public bool Correct()
        {
            bool changed = false;

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (var l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        [DataMember]
        public GlazingConstructionTypes Type = GlazingConstructionTypes.Single;

        public static string GetRadianceMaterial(GlazingConstruction c)
        {
            //return "void plastic White_50 0 0 5 0.5 0.5 0.5 0 0";
            //return "void glass glass_70 0 0 3 0.77 0.77 0.77";

            double trans = 1.0;

            foreach (var l in c.Layers)
            {
                GlazingMaterial gl = l.Material as GlazingMaterial;
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
                s.AppendLine(l.Material + " " + l.Thickness + " [m]");
            }
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            // s.AppendLine("Cost[$/m2] = " + this.GetDollar());

            return s.ToString();
        }


        public double GetDollar()
        {
            double dollar = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay.Material as GlazingMaterial;
                if (mat != null)
                {
                    dollar += lay.Thickness * mat.Cost;
                }
            }
            return dollar + this.Cost;
        }
        public  double GetEE()
        {
            double ee = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay.Material as GlazingMaterial;
                if (mat != null)
                {
                    ee += lay.Thickness * mat.Density *
                          mat.EmbodiedEnergy;
                }
            }
            return ee + this.EmbodiedEnergy;
        }
        public  double GetEC()
        {
            double ec = 0.0;
            foreach (var lay in this.Layers)
            {
                var mat = lay.Material as GlazingMaterial;
                if (mat != null)
                {
                    ec += lay.Thickness * mat.Density *
                          mat.EmbodiedCarbon;
                }
            }
            return ec + this.EmbodiedCarbon;
        }
        public  double[] GetDollar(int LifeTime, double area)
        {
            double[] dollar = new double[LifeTime];
            foreach (var lay in this.Layers)
            {

                var mat = lay.Material as GlazingMaterial;
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
        public  double[] GetEE(int LifeTime, double area)
        {
            double[] ee = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                var mat = lay.Material as GlazingMaterial;
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
        public  double[] GetEC( int LifeTime, double area)
        {
            double[] ec = new double[LifeTime];
            foreach (var lay in this.Layers)
            {
                var mat = lay.Material as GlazingMaterial;
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
    }


    #endregion


    #region GlazingConstructionSimple




    [DataContract]
    public class GlazingConstructionSimple : BaseConstruction//BaseMaterial
    {

        [DataMember]
        public double SHGF { get; set; } = 0.837;
        [DataMember]
        [Units("W/m2-k")]
        public double UValue { get; set; } = 0.075;
        [DataMember]
        public double VisibleTransmittance { get; set; } = 0.898;

        public GlazingConstructionSimple() { }
        public GlazingConstructionSimple(string name, string category, string comment, double tvis, double uval, double shgf)
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

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
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

        public  string GetRadianceMaterial()
        {
            double trans = this.VisibleTransmittance;

            double refractiveIndex = 1.52;
            double rad = (Math.Sqrt(0.8402528435 + 0.0072522239 * trans * trans) - 0.9166530661) / 0.0036261119 / trans;
            return String.Format("void glass {2}_glazing 0 0 4 {0} {0} {0} {1}\n", rad, refractiveIndex, this.Name);
        }
        public  double GetDollar()
        {
            return this.Cost;
        }
        public  double GetEE()
        {

            return this.EmbodiedEnergy;
        }
        public  double GetEC()
        {

            return this.EmbodiedCarbon;
        }
        public  double[] GetDollar( int LifeTime, double area)
        {
            double[] dollar = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                dollar[lft] += this.Cost * area;
            }

            return dollar;
        }
        public  double[] GetEE( int LifeTime, double area)
        {
            double[] ee = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ee[lft] += this.EmbodiedEnergy * area;
            }

            return ee;
        }
        public  double[] GetEC( int LifeTime, double area)
        {
            double[] ec = new double[LifeTime];

            for (int lft = 0; lft < LifeTime; lft += this.Life)
            {
                ec[lft] += this.EmbodiedCarbon * area;
            }

            return ec;
        }
    }


#endregion



    [DataContract]
    public class BaseConstruction : LibraryComponent
    {

        [DataMember]
        [Units("MJ/m2")]
        public double EmbodiedEnergy { get; set; } = 0;

        //[DataMember]
        //public double EmbodiedEnergyStdDev { get; set; }

        [DataMember]
        [Units("kgCO2eq/m2")]
        public double EmbodiedCarbon { get; set; } = 0;

        //[DataMember]
        //public double EmbodiedCarbonStdDev { get; set; }

        [DataMember]
        [Units("$/m2")]
        public double Cost { get; set; } = 0;

        [DataMember]
        [Units("yr")]
        public int Life { get; set; } = 1;

        public BaseConstruction() { }

        public override string ToString() { return Name; }
    }


}
