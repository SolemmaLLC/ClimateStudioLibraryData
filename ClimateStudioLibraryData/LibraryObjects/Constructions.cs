using CSEnergyLib.LibraryObjects;
using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CSEnergyLib.LibraryObjects
{

    #region OpaqueConstruction

    [DataContract]
    [ProtoContract]

    public class CSOpaqueConstruction : CSBaseConstruction
    {
        [DataMember]
        [ProtoMember(1, AsReference = true)]
        public List<Layer<CSOpaqueMaterial>> Layers { get; set; } = new List<Layer<CSOpaqueMaterial>>();



        [DataMember, DefaultValue(ConstructionCategory.Facade)]
        [ProtoMember(2)]
        public ConstructionCategory Type { get; set; } = ConstructionCategory.Facade;




        public CSOpaqueConstruction() { }

        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (Layer<CSOpaqueMaterial> l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        public string GetInfo() {

            StringBuilder s = new StringBuilder();
            s.AppendLine(GetIndicators());          
            s.AppendLine("Layers: [Outside - Inside]");
            s.AppendLine(GetLayers());
             

            return s.ToString();
        }

        public string GetIndicators()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine("U-Value[W/m2K] = " + Math.Round(this.GetUval(), 3));
            s.AppendLine("Heat Capacity[kJ/m2K] = " + Math.Round(this.GetHeatCapacity(), 3));
            s.AppendLine("Embodied Energy[MJ/m2] = " + this.GetEE());
            s.AppendLine("Embodied Carbon[kgCO2/m2] = " + this.GetEC());
            return s.ToString();
        }

        public string GetLayers() {
            StringBuilder s = new StringBuilder();

            int cnt = 1;
            foreach (var l in this.Layers)
            {
                string prefix = " " + cnt + " - ";
                //if(cnt ==1 ) prefix = " O - ";
                //else if (cnt == this.Layers.Count) prefix = " I - ";
                s.AppendLine(prefix + l.Material + " " + l.Thickness + " [m]");
                cnt++;
            }


            return s.ToString();
        }

        public string GetInfoSingleLine()
        {

            StringBuilder s = new StringBuilder();
            s.Append(this.Name+", ");
            s.Append("UVal: " + Math.Round(this.GetUval(), 3) + " [W/m2K], ");
            s.Append("HC: " + Math.Round(this.GetHeatCapacity(),0) + " [kJ/m2K]");
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
                CSOpaqueMaterial glmat = lay.Material;
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
                CSOpaqueMaterial glmat = lay.Material;
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
                CSOpaqueMaterial glmat = lay.Material;
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

        public double GetThickness()
        {
            double thick = 0.0;
            foreach (var lay in this.Layers)
            {
                thick += lay.Thickness ;
            }
             return thick  ;
        }

        public static CSOpaqueConstruction QuickConstruction(string name, ConstructionCategory type, string[] layers, double[] thickness, string category, string source, ref CSLibrary Library)
        {

            CSOpaqueConstruction oc = new CSOpaqueConstruction();
            for (int i = 0; i < layers.Length; i++)
            {
                try
                {
                    if (thickness.Length != layers.Length) { continue; }
                    if (!(thickness[i] > 0)) { continue; }

                    if (Library.OpaqueMaterials.Any(x => x.Name == layers[i]))
                    {

                        var mat = Library.OpaqueMaterials.First(o => o.Name == layers[i]);
                        Layer<CSOpaqueMaterial> layer = new Layer<CSOpaqueMaterial>(thickness[i], mat);
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
        public static CSOpaqueConstruction MixConstructions(string newName, CSOpaqueConstruction c1, CSOpaqueConstruction c2, double areaFractionC2)
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


            List<Layer<CSOpaqueMaterial>> newLayerset = new List<Layer<CSOpaqueMaterial>>();
            for (int i = 0; i < c1.Layers.Count; i++)
            {
                if (i == minLamdaC1Index) newLayerset.Add(new Layer<CSOpaqueMaterial>(newThickness, c1.Layers[i].Material));
                else newLayerset.Add(new Layer<CSOpaqueMaterial>(c1.Layers[i].Thickness, c1.Layers[i].Material));
            }

            CSOpaqueConstruction c = new CSOpaqueConstruction()
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


        public static CSOpaqueConstruction fromJSON(string json)
        {
            return Serialization.Deserialize<CSOpaqueConstruction>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSOpaqueConstruction>(this);
        }
    }

    #endregion


  



    [DataContract]
    [ProtoContract]
    [ProtoInclude(4100, typeof(CSOpaqueConstruction))]
    [ProtoInclude(4200, typeof(CSGlazingConstruction))]
    [ProtoInclude(4300, typeof(CSGlazingConstructionSimple))]

    public class CSBaseConstruction : LibraryComponent
    {


        // system info
        [DataMember, DefaultValue("")]
        [ProtoMember(1)]
        public string Manufacturer { get; set; } = "";


        [DataMember, DefaultValue("")]
        [ProtoMember(2)]
        public string ProductName { get; set; } = "";


        [DataMember, DefaultValue("")]
        [ProtoMember(3)]
        public string Appearance { get; set; } = "";


        [DataMember, DefaultValue(0.0), Units("MJ/m2")]
        [ProtoMember(4)]
        public double EmbodiedEnergy { get; set; } = 0;


        //[DataMember, DefaultValue(0.0)]
        //[ProtoMember(5)]
        //public double EmbodiedEnergyStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0),]
        [Units("kgCO2eq/m2")]
        [ProtoMember(6)]
        public double EmbodiedCarbon { get; set; } = 0;

        //[DataMember, DefaultValue(0.0)]
        //[ProtoMember(7)]
        //public double EmbodiedCarbonStdDev { get; set; } = 0;

        [DataMember, DefaultValue(0.0), Units("$/m2")]
        [ProtoMember(8)]
        public double Cost { get; set; } = 0;

        [DataMember, DefaultValue(10), Units("yr")]
        [ProtoMember(9)]
        public int Life { get; set; } = 10;



        public CSBaseConstruction() { }

        public override string ToString() { return Name; }
    }


}
