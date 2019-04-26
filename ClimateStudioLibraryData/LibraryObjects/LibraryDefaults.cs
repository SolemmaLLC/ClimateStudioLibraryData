using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ArchsimLib.LibraryObjects
{
    public static class LibraryDefaults
    {
        public static Library repairSched(Library lib)
        {

            var version1 = new Version(lib.Version);
            var version2 = new Version("4.2.0.0");

            var result = version1.CompareTo(version2);
            if (result < 0)
            {
                var defLib = loadDefualtLibrary();

                foreach (var d in defLib.DaySchedules)
                {
                    lib.Add(d);
                }
                foreach (var d in defLib.YearSchedules)
                {
                    lib.Add(d);
                }
            }

            return lib;
        }

        public static Library loadDefualtLibrary()
        {
            return LibraryDefaults.getHardCodedDefaultLib();
        }

        //wipes library and restores it with hard coded libray
        public static Library resetLibrary()
        {
            Library l = getHardCodedDefaultLib();

            //writeDefaultLibrary(l);

            return l;
        }


        // hardcoded default library
        public static Library getHardCodedDefaultLib()
        {
            Library Library = new Library();

            #region DEFAULTS - MUST BE IN LIBRARY

            OpaqueMaterial defaultMat = new OpaqueMaterial()
            {
                Name = "defaultMat",
                Category = "Concrete",
                Conductivity = 2.30,
                Density = 2400,
                SpecificHeat = 840,
                ThermalAbsorptance = 0.9,
                SolarAbsorptance = 0.7,
                VisibleAbsorptance = 0.7
            };
            Library.Add(defaultMat);

            GlazingMaterial defaultGMat = new GlazingMaterial()
            {
                Name = "defaultGlazingMat",
                Type = "Uncoated",
                Conductivity = 0.9,
                Density = 2500,
                SolarTransmittance = .775,
                SolarReflectanceFront = .071,
                SolarReflectanceBack = .071,
                VisibleTransmittance = .881,
                VisibleReflectanceFront = .080,
                VisibleReflectanceBack = .080,
                IRTransmittance = 0.00,
                IREmissivityFront = 0.84,
                IREmissivityBack = 0.84
            };
            Library.Add(defaultGMat);



            Layer<OpaqueMaterial> defaultLay = new Layer<OpaqueMaterial>(0.25, defaultMat);
            OpaqueConstruction defaultConstruction = new OpaqueConstruction();
            defaultConstruction.Layers.Add(defaultLay);
            defaultConstruction.Name = "defaultConstruction";
            defaultConstruction.Type = ConstructionCategory.Facade;// "Facade";
            Library.Add(defaultConstruction);


            Layer<WindowMaterialBase> defaultGLay = new Layer<WindowMaterialBase>(0.006, defaultGMat);
            GlazingConstruction defaultGlazing = new GlazingConstruction();
            defaultGlazing.Layers.Add(defaultGLay);
            defaultGlazing.Name = "defaultGlazing";
            defaultGlazing.Type = GlazingConstructionTypes.Single;// "Single";
            defaultGlazing.UValue = "5.894";
            defaultGlazing.SHGF = "0.905";
            defaultGlazing.TVis = "0.913";

            Library.Add(defaultGlazing);


            //AIRWALL
            GlazingMaterial AirWallMat = new GlazingMaterial()
            {
                Name = "100TRANS",
                Type = "uncoated",
                Conductivity = 5,
                Density = 0.0001,
                SolarTransmittance = 0.99,
                SolarReflectanceFront = 0.005,
                SolarReflectanceBack = 0.005,
                VisibleTransmittance = 0.99,
                VisibleReflectanceFront = 0.005,
                VisibleReflectanceBack = 0.005,
                IRTransmittance = 0.99,
                IREmissivityFront = 0.005,
                IREmissivityBack = 0.005
            };
            Library.Add(AirWallMat);
            Layer<WindowMaterialBase> airwallLayer = new Layer<WindowMaterialBase>(0.003, AirWallMat);
            GlazingConstruction airWall = new GlazingConstruction();
            airWall.Layers.Add(airwallLayer);
            airWall.Name = "Airwall";
            airWall.Type = GlazingConstructionTypes.Other;// "Other";
            airWall.UValue = "NA";
            airWall.SHGF = "0.99";
            airWall.TVis = "0.99";
            Library.Add(airWall);




            //---------------------------------------------------------------------------------gases

            // add all possible gas materials to GasMaterials
            // string[] gases = { "AIR", "ARGON", "KRYPTON", "XENON", "SF6" };
            Library.GasMaterials.Clear();
            foreach (var s in Enum.GetValues(typeof(GasTypes)).Cast<GasTypes>())
            {
                Library.GasMaterials.Add(new GasMaterial(s));
            }



            int[] MonthFrom = { 1 };
            int[] DayFrom = { 1 };
            int[] MonthTo = { 12 };
            int[] DayTo = { 31 };



            YearSchedule.QuickSchedule("AirSpeed 0",   new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.AnyNumber, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.AirSpeed), "DefaultLibrary", ref Library);
            YearSchedule.QuickSchedule("AirSpeed 0.1", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.AnyNumber, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.AirSpeed), "DefaultLibrary", ref Library);
            YearSchedule.QuickSchedule("AirSpeed 0.2", new double[] { 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2 }, ScheduleType.AnyNumber, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.AirSpeed), "DefaultLibrary", ref Library);



            double[] hourlyAllOnArr = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            YearSchedule.QuickSchedule("AllOn", hourlyAllOnArr, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Availability), "DefaultLibrary", ref Library);


            double[] hourlyAllOffArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            YearSchedule.QuickSchedule("AllOff", hourlyAllOffArr, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Availability), "DefaultLibrary", ref Library);


            double[] hourlyS21B188to18 = { 18, 18, 18, 18, 18, 18, 18, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 18, 18, 18, 18, 18, 18 };
            YearSchedule.QuickSchedule("S21 B18 8to18", hourlyS21B188to18, ScheduleType.Temperature, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Control), "DefaultLibrary", ref Library);
            double[] hourlyS25B308to18 = { 30, 30, 30, 30, 30, 30, 30, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 30, 30, 30, 30, 30, 30 };
            YearSchedule.QuickSchedule("S25 B30 8to18", hourlyS25B308to18, ScheduleType.Temperature, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Control), "DefaultLibrary", ref Library);


            Library.Add(new ZoneLoad() { Name = "Off", PeopleIsOn = false, EquipmentIsOn = false, LightsIsOn = false, PeopleDensity = 0, IlluminanceTarget = 0, LightingPowerDensity = 0, EquipmentPowerDensity = 0, OccupancySchedule = "AllOn", EquipmentAvailibilitySchedule = "AllOn", LightsAvailibilitySchedule = "AllOn", Category = "Default", DataSource = "Default" });
            Library.Add(new ZoneConditioning() { Name = "Off", HeatIsOn = false, CoolIsOn = false, MechVentIsOn = false, HeatingSetpoint = 19, CoolingSetpoint = 26, MinFreshAirPerson = 0, MinFreshAirArea = 0, Category = "Default" });
            Library.Add(new ZoneVentilation() { Name = "Off", InfiltrationIsOn = false, InfiltrationAch = 0.15, Category = "Infiltration" });
            Library.Add(new DomHotWater() { Name = "Off", IsOn = false, FlowRatePerPerson = 0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            Library.Add(new ZoneConstruction() { Name = "Default", RoofConstruction = "defaultConstruction", FacadeConstruction = "defaultConstruction", SlabConstruction = "defaultConstruction", GroundConstruction = "defaultConstruction", PartitionConstruction = "defaultConstruction" });




            #endregion



            Library.Add(new OpaqueMaterialAirGap() { Name = "VerticalAirGap13mm", Resistance = 0.15, Category = "AirGap", Comment = "Direction of Heatflow = Vertical Up, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialAirGap() { Name = "HorizontalAirGap13mm", Resistance = 0.16, Category = "AirGap", Comment = "Direction of Heatflow = Horizontal, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialAirGap() { Name = "VerticalAirGap20mm", Resistance = 0.15, Category = "AirGap", Comment = "Direction of Heatflow = Vertical Up, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialAirGap() { Name = "HorizontalAirGap20mm", Resistance = 0.18, Category = "AirGap", Comment = "Direction of Heatflow = Horizontal, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialAirGap() { Name = "VerticalAirGap40mm", Resistance = 0.16, Category = "AirGap", Comment = "Direction of Heatflow = Vertical Up, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialAirGap() { Name = "HorizontalAirGap40mm", Resistance = 0.18, Category = "AirGap", Comment = "Direction of Heatflow = Horizontal, EffEmissivity=0.82, MeanTemp=10, TempDiff=5.6", DataSource = "ASHRAE HOF" });


            Library.Add(new OpaqueMaterialNoMass() { Name = "CarpetFibrousFad", Resistance = 0.37, Category = "NoMass", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialNoMass() { Name = "CarpetRubberPad", Resistance = 0.22, Category = "NoMass", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialNoMass() { Name = "CorkTile", Resistance = 0.049, Category = "NoMass", DataSource = "ASHRAE HOF" });
            Library.Add(new OpaqueMaterialNoMass() { Name = "Vaporseal", Resistance = 0.002, Category = "NoMass", DataSource = "ASHRAE HOF" });



            #region  OpaqueMaterialsWithEEIndicators


            Library.Add(new OpaqueMaterial() { Name = @"Pine wood", Category = @"Timber", Conductivity = 0.13, Density = 520, SpecificHeat = 1600, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Douglas fir", Category = @"Timber", Conductivity = 0.12, Density = 530, SpecificHeat = 1600, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Oak", Category = @"Timber", Conductivity = 0.18, Density = 690, SpecificHeat = 2400, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Spruce", Category = @"Timber", Conductivity = 0.13, Density = 450, SpecificHeat = 1600, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Larch", Category = @"Timber", Conductivity = 0.13, Density = 460, SpecificHeat = 1600, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Oriented strand board", Category = @"Timber", Conductivity = 0.13, Density = 650, SpecificHeat = 1700, EmbodiedEnergy = 15, EmbodiedCarbon = 0.96, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.45fos + 0.54 bio- embodied carbon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Medium density fiberboard", Category = @"Timber", Conductivity = 0.09, Density = 500, SpecificHeat = 1700, EmbodiedEnergy = 11, EmbodiedCarbon = 0.72, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Chipboard", Category = @"Timber", Conductivity = 0.14, Density = 650, SpecificHeat = 1800, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Concrete", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE, General Concrete data]" });
            Library.Add(new OpaqueMaterial() { Name = @"General Concrete", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE, General Concrete data]" });
            Library.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 20-30 MPa", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 30-50 MPa", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, EmbodiedEnergy = 0.74, EmbodiedCarbon = 0.099, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.Add(new OpaqueMaterial() { Name = @"Asphalt low binder content", Category = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, EmbodiedEnergy = 3.39, EmbodiedCarbon = 0.191, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.Add(new OpaqueMaterial() { Name = @"Asphalt high binder content", Category = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, EmbodiedEnergy = 4.46, EmbodiedCarbon = 0.216, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.Add(new OpaqueMaterial() { Name = @"Lightweight concrete", Category = @"Concrete", Conductivity = 1.3, Density = 1800, SpecificHeat = 1000, EmbodiedEnergy = 0.78, EmbodiedCarbon = 0.106, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Concrete 20/25 Mpa]" });
            Library.Add(new OpaqueMaterial() { Name = @"Cement screed", Category = @"Screed", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, EmbodiedEnergy = 1.33, EmbodiedCarbon = 0.221, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Mortar (1:3 cement:sand mix) ]" });
            Library.Add(new OpaqueMaterial() { Name = @"Basalt", Category = @"Masonry", Conductivity = 3.5, Density = 2850, SpecificHeat = 1000, EmbodiedEnergy = 1.26, EmbodiedCarbon = 0.073, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Stone]" });
            Library.Add(new OpaqueMaterial() { Name = @"Granite", Category = @"Masonry", Conductivity = 2.8, Density = 2600, SpecificHeat = 790, EmbodiedEnergy = 11, EmbodiedCarbon = 0.64, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Granite]" });
            Library.Add(new OpaqueMaterial() { Name = @"Lime stone", Category = @"Masonry", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, EmbodiedEnergy = 1.5, EmbodiedCarbon = 0.087, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Limestone]" });
            Library.Add(new OpaqueMaterial() { Name = @"Adobe 1500kg_m3", Category = @"Masonry", Conductivity = 0.66, Density = 1500, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA ICE,  General Clay data]" });
            Library.Add(new OpaqueMaterial() { Name = @"Sand stone", Category = @"Masonry", Conductivity = 2.3, Density = 2600, SpecificHeat = 710, EmbodiedEnergy = 1, EmbodiedCarbon = 0.058, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Sandstone]" });
            Library.Add(new OpaqueMaterial() { Name = @"Clinker brick 1400kg_m3", Category = @"Masonry", Conductivity = 0.58, Density = 1400, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General (Common Brick)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Clinker brick 1600kg_m3", Category = @"Masonry", Conductivity = 0.68, Density = 1600, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Clinker brick 1800kg_m3", Category = @"Masonry", Conductivity = 0.81, Density = 1800, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Clinker brick 2000kg_m3", Category = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Aerated concrete 350kg_m3", Category = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Aerated concrete 500kg_m3", Category = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 1.6-0.30", Category = @"Masonry", Conductivity = 0.08, Density = 300, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Poroton Plan-T10", Category = @"Masonry", Conductivity = 0.1, Density = 650, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Common Brick]" });
            Library.Add(new OpaqueMaterial() { Name = @"Rigid foam  EPS 035", Category = @"Insulation", Conductivity = 0.035, Density = 30, SpecificHeat = 1500, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR no coating", Category = @"Insulation", Conductivity = 0.03, Density = 30, SpecificHeat = 1400, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR alu coating", Category = @"Insulation", Conductivity = 0.025, Density = 30, SpecificHeat = 1400, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR fleece coating", Category = @"Insulation", Conductivity = 0.028, Density = 30, SpecificHeat = 1400, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.Add(new OpaqueMaterial() { Name = @"Wood fiber insulating board", Category = @"Insulation", Conductivity = 0.042, Density = 160, SpecificHeat = 2100, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.Add(new OpaqueMaterial() { Name = @"Styrofoam", Category = @"Insulation", Conductivity = 0.04, Density = 20, SpecificHeat = 1500, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] No data found" });
            Library.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Variotec", Category = @"Insulation", Conductivity = 0.007, Density = 205, SpecificHeat = 900, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, General Insulation]" });
            Library.Add(new OpaqueMaterial() { Name = @"Wood wool board 15mm", Category = @"Timber", Conductivity = 0.09, Density = 570, SpecificHeat = 2100, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Wood wool board 25mm", Category = @"Timber", Conductivity = 0.09, Density = 460, SpecificHeat = 2100, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Wood wool board 35mm", Category = @"Timber", Conductivity = 0.09, Density = 415, SpecificHeat = 2100, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA, ICE, Woodwool (Board)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Wood wool board 50mm", Category = @"Timber", Conductivity = 0.09, Density = 390, SpecificHeat = 2100, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Light adobe NF 700", Category = @"Masonry", Conductivity = 0.21, Density = 700, SpecificHeat = 1200, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1200", Category = @"Masonry", Conductivity = 0.47, Density = 1200, SpecificHeat = 1200, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1800", Category = @"Masonry", Conductivity = 0.91, Density = 1800, SpecificHeat = 1200, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.Add(new OpaqueMaterial() { Name = @"Shale", Category = @"Masonry", Conductivity = 2.2, Density = 2400, SpecificHeat = 760, EmbodiedEnergy = 0.03, EmbodiedCarbon = 0.002, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Shale]" });
            Library.Add(new OpaqueMaterial() { Name = @"Clinker brick", Category = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.35", Category = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.40", Category = @"Masonry", Conductivity = 0.1, Density = 400, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.50", Category = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.55", Category = @"Masonry", Conductivity = 0.14, Density = 550, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.60", Category = @"Masonry", Conductivity = 0.16, Density = 600, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 6-0.65", Category = @"Masonry", Conductivity = 0.18, Density = 650, SpecificHeat = 1000, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Poroton T12", Category = @"Masonry", Conductivity = 0.12, Density = 650, SpecificHeat = 1000, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.Add(new OpaqueMaterial() { Name = @"Cork", Category = @"Insulation", Conductivity = 0.05, Density = 160, SpecificHeat = 1800, ThermalAbsorptance = 0.9, SolarAbsorptance = 0.78, VisibleAbsorptance = 0.78, EmbodiedEnergy = 4, EmbodiedCarbon = 0.19, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ThermalEmittance SolarAbsorptance VisibleAbsorptance: DesignBuilderv3] [LCA, ICE Cork]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong Multipor DAA ds", Category = @"Insulation", Conductivity = 0.047, Density = 115, SpecificHeat = 1300, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WI WTR DI", Category = @"Insulation", Conductivity = 0.042, Density = 90, SpecificHeat = 1300, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WAP", Category = @"Insulation", Conductivity = 0.045, Density = 110, SpecificHeat = 1300, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.Add(new OpaqueMaterial() { Name = @"Foam glass", Category = @"Insulation", Conductivity = 0.056, Density = 130, SpecificHeat = 750, EmbodiedEnergy = 27, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Celular Glass]" });
            Library.Add(new OpaqueMaterial() { Name = @"Reed", Category = @"Insulation", Conductivity = 0.065, Density = 225, SpecificHeat = 1200, EmbodiedEnergy = 0.24, EmbodiedCarbon = 0.01, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ LCA, ICE, Straw]" });
            Library.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Vacupor  NT-B2-S", Category = @"Insulation", Conductivity = 0.007, Density = 190, SpecificHeat = 1050, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });

            Library.Add(new OpaqueMaterial() { Name = @"Stainless Steel", Category = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalAbsorptance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 56.7, EmbodiedCarbon = 6.15, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Stainless Steel]" });
            Library.Add(new OpaqueMaterial() { Name = @"Steel", Category = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalAbsorptance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 20.1, EmbodiedCarbon = 1.46, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, General Steel ]" });

            Library.Add(new OpaqueMaterial() { Name = @"Rammed Earth", Category = @"Masonry", Conductivity = 0.75, Density = 1730, SpecificHeat = 880, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @" [lambda  rho c: U-Wert.net] [LCA, ICE Mud] [LCA, ICE, Single Clay Brick]" });


            //double check EE values
            Library.Add(new OpaqueMaterial() { Name = @"Gypsum Board", Category = @"Boards", Conductivity = 0.32, Density = 1000, SpecificHeat = 1100, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: Saint-Gobain Rigips][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Cross Laminated Timber", Category = @"Timber", Conductivity = 0.13, Density = 500, SpecificHeat = 1600, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: dataholz.com][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.Add(new OpaqueMaterial() { Name = @"Plaster", Category = @"Screed", Conductivity = 1.0, Density = 2000, SpecificHeat = 1130, EmbodiedEnergy = 1.33, EmbodiedCarbon = 0.221, Cost = 0, Comment = @"[lambda  rho c: dataholz.com] [ LCA ICE, Mortar (1:3 cement:sand mix) ]" });
            Library.Add(new OpaqueMaterial() { Name = @"Mineral Wool", Category = @"Insulation", Conductivity = 0.041, Density = 155, SpecificHeat = 1130, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: dataholz.com]" });
            Library.Add(new OpaqueMaterial() { Name = @"XPS Board", Category = "Insulation", Conductivity = 0.034, Density = 35, SpecificHeat = 1400, EmbodiedEnergy = 87.4, EmbodiedCarbon = 2.8, Cost = 0.0, Comment = "" });
            Library.Add(new OpaqueMaterial() { Name = @"Sand-Lime Brick", Category = "Masonry", Conductivity = 0.56, Density = 1200, SpecificHeat = 1000, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "" });
            Library.Add(new OpaqueMaterial() { Name = @"Bonded chippings", Category = @"Screed", Conductivity = 0.7, Density = 1800, SpecificHeat = 1000, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "" });
            Library.Add(new OpaqueMaterial() { Name = @"Impact sound insulation", Category = "Insulation", Conductivity = 0.035, Density = 120, SpecificHeat = 1030, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0.0, Comment = "" });
            Library.Add(new OpaqueMaterial() { Name = @"Cellulose", Category = "Insulation", Conductivity = .042, Density = 42.5, SpecificHeat = 1380, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "No comment" });



            var tarr = new double[] { -20, 20, 20.5, 100 }.ToList();
            var entarr = new double[] { 0.01, 33400, 70000, 137000 }.ToList();
            Library.Add(new OpaqueMaterial() { Name = @"Plaster Board with PCM", Category = "Boards", Roughness = "Smooth", Conductivity = 0.7264224, Density = 1601.846, SpecificHeat = 836.8, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "No comment", PhaseChange = true, TemperatureArray = tarr, EnthalpyArray = entarr, DataSource = "EnergyPlus" });










            #endregion





            //Basic wall
            OpaqueConstruction.QuickConstruction("120mmInsulation 200mmConcrete", ConstructionCategory.Facade, new string[] { "XPS Board", "General Concrete" }, new double[] { 0.12, 0.20 }, "Concrete", "", ref Library);
            //Basic roof
            OpaqueConstruction.QuickConstruction("300mmInsulation 200mmConcrete", ConstructionCategory.Facade, new string[] { "XPS Board", "General Concrete" }, new double[] { 0.30, 0.20 }, "Concrete", "", ref Library);
            //Basic floor
            OpaqueConstruction.QuickConstruction("200mmConcrete", ConstructionCategory.InteriorFloor, new string[] { "General Concrete" }, new double[] { 0.20 }, "Concrete", "", ref Library);
            //Basic partition
            OpaqueConstruction.QuickConstruction("115mmSandLimeBrick", ConstructionCategory.Partition, new string[] { "Plaster", "Sand-Lime Brick", "Plaster" }, new double[] { 0.005, 0.08, 0.08 }, "Concrete", "", ref Library);
            //Basic ground
            OpaqueConstruction.QuickConstruction("300mmConcrete 80mmInsulation 80mmScreed", ConstructionCategory.Partition, new string[] { "General Concrete", "XPS Board", "Cement screed" }, new double[] { 0.3, 0.115, 0.005 }, "Concrete", "", ref Library);


            //Solid wood constructions
            OpaqueConstruction.QuickConstruction("300mmInsulation 94mmSolidWood 24mmGypsum", ConstructionCategory.Facade, new string[] { "Medium density fiberboard", "Wood fiber insulating board", "Cross Laminated Timber", "Wood fiber insulating board", "Gypsum Board" }, new double[] { 0.015, 0.3, 0.094, 0.08, 0.0245 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("120mmInsulation 78mmSolidWood 13mmGypsum", ConstructionCategory.Facade, new string[] { "Plaster", "Mineral Wool", "Cross Laminated Timber", "Gypsum Board" }, new double[] { 0.004, 0.12, 0.078, 0.013 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("160mmInsulation 94mmSolidWood", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Mineral Wool", "Cross Laminated Timber" }, new double[] { 0.08, 0.08, 0.094 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("160mmInsulation 94mmSolidWood", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Mineral Wool", "Cross Laminated Timber" }, new double[] { 0.08, 0.08, 0.094 }, "Timber", "dataholz.com", ref Library);

            OpaqueConstruction.QuickConstruction("12mmGypsum 78mmSolidWood 12mmGypsum", ConstructionCategory.Partition, new string[] { "Gypsum Board", "Cross Laminated Timber", "Gypsum Board" }, new double[] { 0.012, 0.78, 0.012 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("78mmSolidWood", ConstructionCategory.Partition, new string[] { "Cross Laminated Timber" }, new double[] { 0.78 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("150mmScreedWithImpactSoundInsulation 140mmSolidWood", ConstructionCategory.InteriorFloor, new string[] { "Cross Laminated Timber", "Bonded chippings", "Impact sound insulation", "Cement screed" }, new double[] { 0.14, 0.06, 0.03, 0.06 }, "Timber", "dataholz.com, gdmnxn02-00", ref Library);



            OpaqueConstruction.QuickConstruction("Partition_Mass", ConstructionCategory.Facade, new string[] { "Plaster", "Poroton T12", "Plaster" }, new double[] { 0.05, 0.24, 0.05 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("Partition_Light", ConstructionCategory.Facade, new string[] { "Gypsum Board" }, new double[] { 0.040 }, "Light", "", ref Library);

            OpaqueConstruction.QuickConstruction("Slab_Mass", ConstructionCategory.Facade, new string[] { "General Concrete" }, new double[] { 0.2 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("Slab_Light", ConstructionCategory.Facade, new string[] { "Cross Laminated Timber" }, new double[] { 0.2 }, "Light", "", ref Library);

            //By Uvalue
            OpaqueConstruction.QuickConstruction("UVal_0.1_Mass", ConstructionCategory.Facade, new string[] { "Mineral Wool", "General Concrete" }, new double[] { 0.4, 0.20 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.1_Light", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Gypsum Board" }, new double[] { 0.4, 0.020 }, "Light", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.2_Mass", ConstructionCategory.Facade, new string[] { "Mineral Wool", "General Concrete" }, new double[] { 0.194, 0.20 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.2_Light", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Gypsum Board" }, new double[] { 0.195, 0.020 }, "Light", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.3_Mass", ConstructionCategory.Facade, new string[] { "Mineral Wool", "General Concrete" }, new double[] { 0.1255, 0.20 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.3_Light", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Gypsum Board" }, new double[] { 0.127, 0.020 }, "Light", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.4_Mass", ConstructionCategory.Facade, new string[] { "Mineral Wool", "General Concrete" }, new double[] { 0.915, 0.20 }, "Mass", "", ref Library);
            OpaqueConstruction.QuickConstruction("UVal_0.4_Light", ConstructionCategory.Facade, new string[] { "Mineral Wool", "Gypsum Board" }, new double[] { 0.929, 0.020 }, "Light", "", ref Library);


            //List of premade basic glazing materials/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            GlazingMaterial gm1 = new GlazingMaterial()
            {
                Name = "Generic Clear Glass 6mm",
                Type = "Uncoated",
                Conductivity = 0.9,
                Density = 2500,
                EmbodiedEnergy = 15,
                EmbodiedCarbon = 0.85,
                Cost = 0.0,
                Comment = "",
                SolarTransmittance = 0.68,
                SolarReflectanceFront = 0.09,
                SolarReflectanceBack = 0.10,
                VisibleTransmittance = 0.81,
                VisibleReflectanceFront = 0.11,
                VisibleReflectanceBack = 0.12,
                IRTransmittance = 0.00,
                IREmissivityFront = 0.84,
                IREmissivityBack = 0.20
            };
            Library.Add(gm1);

            //List of premade basic glazing constructions/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Basic double glazing
            Layer<WindowMaterialBase> go1 = new Layer<WindowMaterialBase>(0.006, gm1);
            Layer<WindowMaterialBase> go2 = new Layer<WindowMaterialBase>(0.006, Library.GasMaterials.Single(o => o.Name == "AIR"));

            GlazingConstruction gc1 = new GlazingConstruction();

            gc1.Layers.Add(go1);
            gc1.Layers.Add(go2);
            gc1.Layers.Add(go1);
            gc1.Name = "DblClear Air 6_6_6";
            gc1.Type = GlazingConstructionTypes.Double;// "Double";
            gc1.UValue = "2.67";
            gc1.SHGF = "0.703";
            gc1.TVis = "0.781";
            Library.Add(gc1);




            #region schedules

            //--------------------------------------------------------------------------------schedules


            YearSchedule.QuickSchedule("occResidential", new double[] { 1, 1, 1, 1, 1, 1, 0.8, 0.6, 0.4, 0.4, 0.4, 0.6, 0.8, 0.6, 0.4, 0.4, 0.6, 0.8, 0.8, 0.8, 0.8, 1, 1, 1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipResidential", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.5, 1, 0.5, 0.5, 0.5, 1, 1, 0.5, 0.5, 0.5, 1, 1, 1, 1, 0.5, 0.5, 0.5, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsResidential", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occKitchen", new double[] { 0, 0, 0, 0, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 1, 0.6, 0.4, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipKitchen", new double[] { 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.8, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.2, 0.2, 0.2 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsKitchen", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);

            YearSchedule.QuickSchedule("occHotelRoom", new double[] { 1, 1, 1, 1, 1, 1, 0.8, 0.4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.4, 0.6, 0.8, 1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipHotelRoom", new double[] { 1, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.6, 0.8, 1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsHotelRoom", new double[] { 0, 0, 0, 0, 0, 1, 0.8, 0.4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.4, 0.6, 0.8, 1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipOffice", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6, 1, 0.4, 0, 0, 0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipMeetingRoom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occClassRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.8, 0.8, 0.4, 0.2, 0.4, 0.8, 0.8, 0.2, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipClassRoom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.8, 0.8, 0.4, 0.2, 0.4, 0.8, 0.8, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsClassRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipLibrary", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipLectureHall", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipSuperMarket", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Occupancy), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipShopping", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Equipment), "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, ScheduleType.Fraction, Enum.GetName(typeof(ScheduleCategory), ScheduleCategory.Lights), "based on SIA Merkblatt 2024 Occ Schedule", ref Library);




            var DHW_Residential = Library.Add(new DomHotWater() { IsOn = true, Name = "Residential", FlowRatePerPerson = 40.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });  // l / h / P
            var DHW_Kitchen = Library.Add(new DomHotWater() { IsOn = true, Name = "Kitchen", FlowRatePerPerson = 20.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_Office = Library.Add(new DomHotWater() { IsOn = true, Name = "Office", FlowRatePerPerson = 10.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_MeetingRoom = Library.Add(new DomHotWater() { IsOn = true, Name = "MeetingRoom", FlowRatePerPerson = 0.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_HotelRoom = Library.Add(new DomHotWater() { IsOn = true, Name = "HotelRoom", FlowRatePerPerson = 40.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_ClassRoom = Library.Add(new DomHotWater() { IsOn = true, Name = "ClassRoom", FlowRatePerPerson = 2.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_Library = Library.Add(new DomHotWater() { IsOn = true, Name = "Library", FlowRatePerPerson = 2.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_LectureHall = Library.Add(new DomHotWater() { IsOn = true, Name = "LectureHall", FlowRatePerPerson = 2.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_SuperMarket = Library.Add(new DomHotWater() { IsOn = true, Name = "SuperMarket", FlowRatePerPerson = 2.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_Shopping = Library.Add(new DomHotWater() { IsOn = true, Name = "Shopping", FlowRatePerPerson = 2.0 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            var DHW_ScienceLab = Library.Add(new DomHotWater() { IsOn = true, Name = "ScienceLab", FlowRatePerPerson = 5 / 24.0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });



            Random r = new Random();
            var Values = new double[8760];
            for (int i = 0; i < 8760; i++) { Values[i] = (double)r.Next(1, 100) / 100.0; }

            Library.Add(new ScheduleArray() { Name = "RandomBehavior", Category = "Random", Values = Values });
            //Library.ArraySchedules.Add(new ScheduleArray() { Name = "RandomBehavior2", Category = "Random", Values = Values });


            #endregion


            var ZL_Residential = Library.Add(new ZoneLoad() { Name = "Residential", PeopleDensity = 1.0 / 40.0, IlluminanceTarget = 200, LightingPowerDensity = 9.5, EquipmentPowerDensity = 2, OccupancySchedule = "occResidential", EquipmentAvailibilitySchedule = "equipResidential", LightsAvailibilitySchedule = "lightsResidential", Category = "Residential", DataSource = "SIA Merkblatt 2024" });
            var ZL_Kitchen = Library.Add(new ZoneLoad() { Name = "Kitchen", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 500, LightingPowerDensity = 17, EquipmentPowerDensity = 40, OccupancySchedule = "occKitchen", EquipmentAvailibilitySchedule = "equipKitchen", LightsAvailibilitySchedule = "lightsKitchen", Category = "Residential", DataSource = "SIA Merkblatt 2024" });
            var ZL_Office = Library.Add(new ZoneLoad() { Name = "Office", PeopleDensity = 1.0 / 14.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 7, OccupancySchedule = "occOffice", EquipmentAvailibilitySchedule = "equipOffice", LightsAvailibilitySchedule = "lightsOffice", Category = "Office", DataSource = "SIA Merkblatt 2024", DimmingType = DimmingItem.Continuous });
            var ZL_MeetingRoom = Library.Add(new ZoneLoad() { Name = "MeetingRoom", PeopleDensity = 1.0 / 3.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 2, OccupancySchedule = "occMeetingRoom", EquipmentAvailibilitySchedule = "equipMeetingRoom", LightsAvailibilitySchedule = "lightsMeetingRoom", Category = "Office", DataSource = "SIA Merkblatt 2024", DimmingType = DimmingItem.Continuous });
            var ZL_HotelRoom = Library.Add(new ZoneLoad() { Name = "HotelRoom", PeopleDensity = 1.0 / 10.0, IlluminanceTarget = 50, LightingPowerDensity = 3, EquipmentPowerDensity = 2, OccupancySchedule = "occHotelRoom", EquipmentAvailibilitySchedule = "equipHotelRoom", LightsAvailibilitySchedule = "lightsHotelRoom", Category = "Residential", DataSource = "SIA Merkblatt 2024" });
            var ZL_ClassRoom = Library.Add(new ZoneLoad() { Name = "ClassRoom", PeopleDensity = 1.0 / 3.0, IlluminanceTarget = 500, LightingPowerDensity = 14, EquipmentPowerDensity = 4, OccupancySchedule = "occClassRoom", EquipmentAvailibilitySchedule = "equipClassRoom", LightsAvailibilitySchedule = "lightsClassRoom", Category = "Residential", DataSource = "SIA Merkblatt 2024", DimmingType = DimmingItem.Continuous });
            var ZL_Library = Library.Add(new ZoneLoad() { Name = "Library", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 200, LightingPowerDensity = 7, EquipmentPowerDensity = 2, OccupancySchedule = "occLibrary", EquipmentAvailibilitySchedule = "equipLibrary", LightsAvailibilitySchedule = "lightsLibrary", Category = "Education", DataSource = "SIA Merkblatt 2024" });
            var ZL_LectureHall = Library.Add(new ZoneLoad() { Name = "LectureHall", PeopleDensity = 1.0 / 2.0, IlluminanceTarget = 500, LightingPowerDensity = 12.5, EquipmentPowerDensity = 4, OccupancySchedule = "occLectureHall", EquipmentAvailibilitySchedule = "equipLectureHall", LightsAvailibilitySchedule = "lightsLectureHall", Category = "Education", DataSource = "SIA Merkblatt 2024" });
            var ZL_SuperMarket = Library.Add(new ZoneLoad() { Name = "SuperMarket", PeopleDensity = 1.0 / 3.0, IlluminanceTarget = 300, LightingPowerDensity = 27.5, EquipmentPowerDensity = 5, OccupancySchedule = "occSuperMarket", EquipmentAvailibilitySchedule = "equipSuperMarket", LightsAvailibilitySchedule = "lightsSuperMarket", Category = "Commercial", DataSource = "SIA Merkblatt 2024" });
            var ZL_Shopping = Library.Add(new ZoneLoad() { Name = "Shopping", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 300, LightingPowerDensity = 33.5, EquipmentPowerDensity = 2, OccupancySchedule = "occShopping", EquipmentAvailibilitySchedule = "equipShopping", LightsAvailibilitySchedule = "lightsShopping", Category = "Commercial", DataSource = "SIA Merkblatt 2024" });
            var ZL_ScienceLab = Library.Add(new ZoneLoad() { Name = "ScienceLab", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 20, OccupancySchedule = "occOffice", EquipmentAvailibilitySchedule = "equipOffice", LightsAvailibilitySchedule = "lightsOffice", Category = "Education", DataSource = "SIA Merkblatt 2024" });


            var ZC_Residential = Library.Add(new ZoneConditioning() { Name = "Residential", HeatingSetpoint = 19, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 30 * 1000 / 3600, MinFreshAirArea = 0.8 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "SIA Merkblatt 2024" });
            var ZC_Kitchen = Library.Add(new ZoneConditioning() { Name = "Kitchen", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 0 * 1000 / 3600, MinFreshAirArea = 20 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "SIA Merkblatt 2024" });
            var ZC_Office = Library.Add(new ZoneConditioning() { Name = "Office", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 36 * 1000 / 3600, MinFreshAirArea = 3.6 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Office", DataSource = "SIA Merkblatt 2024" });
            var ZC_MeetingRoom = Library.Add(new ZoneConditioning() { Name = "MeetingRoom", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 36.0 * 1000 / 3600, MinFreshAirArea = 12 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Office", DataSource = "SIA Merkblatt 2024" });
            var ZC_HotelRoom = Library.Add(new ZoneConditioning() { Name = "HotelRoom", HeatingSetpoint = 21, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 36.0 * 1000 / 3600, MinFreshAirArea = 3.6 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "SIA Merkblatt 2024" });   // "L/s/p"
            var ZC_ClassRoom = Library.Add(new ZoneConditioning() { Name = "ClassRoom", HeatingSetpoint = 21, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 25.0 * 1000 / 3600, MinFreshAirArea = 8.3 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "SIA Merkblatt 2024" });   // "L/s/p"
            var ZC_Library = Library.Add(new ZoneConditioning() { Name = "Library", HeatingSetpoint = 21, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 36.0 * 1000 / 3600, MinFreshAirArea = 7.2 * 1000 / 3600, HumidistatOnOff = true, MaxHumidity = 45, MinHumidity = 35, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "SIA Merkblatt 2024" });   // "L/s/p"
            var ZC_LectureHall = Library.Add(new ZoneConditioning() { Name = "LectureHall", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 30.0 * 1000 / 3600, MinFreshAirArea = 15.0 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "SIA Merkblatt 2024" });
            var ZC_SuperMarket = Library.Add(new ZoneConditioning() { Name = "SuperMarket", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 30.0 * 1000 / 3600, MinFreshAirArea = 10.0 * 1000 / 3600, HumidistatOnOff = true, MaxHumidity = 45, MinHumidity = 35, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Commercial", DataSource = "SIA Merkblatt 2024" });   // "L/s/p"
            var ZC_Shopping = Library.Add(new ZoneConditioning() { Name = "Shopping", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 30.0 * 1000 / 3600, MinFreshAirArea = 6.0 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Commercial", DataSource = "SIA Merkblatt 2024" });
            var ZC_ScienceLab = Library.Add(new ZoneConditioning() { Name = "ScienceLab", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 0, MinFreshAirArea = 5 * 4000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "ETH Zurich Richtlinie Laborbauten" }); //ach 5 minimum


            //Library.Add(new ZoneConditioning() { Name = "ResidentialASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5 , MinFreshAirArea = 0.3, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });
            //Library.Add(new ZoneConditioning() { Name = "KitchenASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 0 * 1000 / 3600, MinFreshAirArea = 20 * 1000 / 3600, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "ANSI/ASHRAE/IES Standard 90.1-2010" });

            Library.Add(new ZoneConditioning() { Name = "HotelRoomASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Residential", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });   // "L/s/p"
            Library.Add(new ZoneConditioning() { Name = "OfficeASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Office", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });
            Library.Add(new ZoneConditioning() { Name = "MeetingRoomASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Office", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });
            Library.Add(new ZoneConditioning() { Name = "ClassRoomASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 5, MinFreshAirArea = 0.6, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });   // "L/s/p"
            Library.Add(new ZoneConditioning() { Name = "LibraryASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.6, HumidistatOnOff = true, MaxHumidity = 45, MinHumidity = 35, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });   // "L/s/p"
            Library.Add(new ZoneConditioning() { Name = "LectureHallASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });
            Library.Add(new ZoneConditioning() { Name = "SuperMarketASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, HumidistatOnOff = true, MaxHumidity = 45, MinHumidity = 35, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Commercial", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });   // "L/s/p"
            Library.Add(new ZoneConditioning() { Name = "ShoppingASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.6, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Commercial", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });
            Library.Add(new ZoneConditioning() { Name = "ScienceLabASHRAE", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 5, MinFreshAirArea = 0.9, HeatRecoveryType = HeatRecoveryItem.Sensible, HeatRecoveryEfficiencySensible = 0.6, Category = "Education", DataSource = "ANSI/ASHRAE STANDARD 62-2001" });




            var ZV_PoorAirTightness = Library.Add(new ZoneVentilation() { Name = "PoorAirTightness", InfiltrationIsOn = true, InfiltrationAch = 1.2, Category = "Infiltration" });
            var ZV_ModerateAirTightness = Library.Add(new ZoneVentilation() { Name = "ModerateAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.5, Category = "Infiltration" });
            var ZV_GoodAirTightness = Library.Add(new ZoneVentilation() { Name = "GoodAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.15, Category = "Infiltration" });




            Library.Add(new ZoneConstruction() { Name = "SolidWood", RoofConstruction = "300mmInsulation 94mmSolidWood 24mmGypsum", FacadeConstruction = "300mmInsulation 94mmSolidWood 24mmGypsum", SlabConstruction = "150mmScreedWithImpactSoundInsulation 140mmSolidWood", GroundConstruction = "120mmInsulation 200mmConcrete", PartitionConstruction = "12mmGypsum 78mmSolidWood 12mmGypsum" });


            var UVal1_Mass = Library.Add(new ZoneConstruction() { Name = "UVal_0.1_Mass", RoofConstruction = "UVal_0.1_Mass", FacadeConstruction = "UVal_0.1_Mass", SlabConstruction = "Slab_Mass", GroundConstruction = "UVal_0.1_Mass", PartitionConstruction = "Partition_Mass" });
            var UVal1_Light = Library.Add(new ZoneConstruction() { Name = "UVal_0.1_Light", RoofConstruction = "UVal_0.1_Light", FacadeConstruction = "UVal_0.1_Light", SlabConstruction = "Slab_Light", GroundConstruction = "UVal_0.1_Light", PartitionConstruction = "Partition_Light" });
            var UVal2_Mass = Library.Add(new ZoneConstruction() { Name = "UVal_0.2_Mass", RoofConstruction = "UVal_0.2_Mass", FacadeConstruction = "UVal_0.2_Mass", SlabConstruction = "Slab_Mass", GroundConstruction = "UVal_0.2_Mass", PartitionConstruction = "Partition_Mass" });
            var UVal2_Light = Library.Add(new ZoneConstruction() { Name = "UVal_0.2_Light", RoofConstruction = "UVal_0.2_Light", FacadeConstruction = "UVal_0.2_Light", SlabConstruction = "Slab_Light", GroundConstruction = "UVal_0.2_Light", PartitionConstruction = "Partition_Light" });
            var UVal3_Mass = Library.Add(new ZoneConstruction() { Name = "UVal_0.3_Mass", RoofConstruction = "UVal_0.3_Mass", FacadeConstruction = "UVal_0.3_Mass", SlabConstruction = "Slab_Mass", GroundConstruction = "UVal_0.3_Mass", PartitionConstruction = "Partition_Mass" });
            var UVal3_Light = Library.Add(new ZoneConstruction() { Name = "UVal_0.3_Light", RoofConstruction = "UVal_0.3_Light", FacadeConstruction = "UVal_0.3_Light", SlabConstruction = "Slab_Light", GroundConstruction = "UVal_0.3_Light", PartitionConstruction = "Partition_Light" });
            var UVal4_Mass = Library.Add(new ZoneConstruction() { Name = "UVal_0.4_Mass", RoofConstruction = "UVal_0.4_Mass", FacadeConstruction = "UVal_0.4_Mass", SlabConstruction = "Slab_Mass", GroundConstruction = "UVal_0.4_Mass", PartitionConstruction = "Partition_Mass" });
            var UVal4_Light = Library.Add(new ZoneConstruction() { Name = "UVal_0.4_Light", RoofConstruction = "UVal_0.4_Light", FacadeConstruction = "UVal_0.4_Light", SlabConstruction = "Slab_Light", GroundConstruction = "UVal_0.4_Light", PartitionConstruction = "Partition_Light" });

            #region SimpleGlass

            Library.Add(new GlazingConstructionSimple() { Name = "SinglePaneClr", Category = "Single", Comment = "Standard clear", VisibleTransmittance = 0.913, UValue = 5.894, SHGF = 0.905 , Cost =436, EmbodiedEnergy = 8, EmbodiedCarbon = 0.5, Life = 30, DataSource = "EnergyPlus, RSMEANS, Citherlet et al" });
            Library.Add(new GlazingConstructionSimple() { Name = "DoublePaneClr", Category = "Double", Comment = "Standard clear", VisibleTransmittance = 0.812, UValue = 2.720, SHGF = 0.764, Cost = 481, EmbodiedEnergy = 20, EmbodiedCarbon = 2, Life = 30, DataSource = "EnergyPlus, RSMEANS, Citherlet et al" });
            Library.Add(new GlazingConstructionSimple() { Name = "DoublePaneLoEe2", Category = "Double", Comment = "Low emissivity coating on layer e2", VisibleTransmittance = 0.444, UValue = 1.493, SHGF = 0.373, Cost = 503, EmbodiedEnergy = 20, EmbodiedCarbon = 2, Life = 30, DataSource = "EnergyPlus, RSMEANS, Citherlet et al" });
            Library.Add(new GlazingConstructionSimple() { Name = "DoublePaneLoEe3", Category = "Double", Comment = "Low emissivity coating on layer e3", VisibleTransmittance = 0.769, UValue = 1.507, SHGF = 0.649, Cost = 503, EmbodiedEnergy = 20, EmbodiedCarbon = 2, Life = 30, DataSource = "EnergyPlus, RSMEANS, Citherlet et al" });
            Library.Add(new GlazingConstructionSimple() { Name = "TriplePaneLoE", Category = "Triple", Comment = "Low emissivity coating on layer e2 and e5", VisibleTransmittance = 0.661, UValue = 0.785, SHGF = 0.764, Cost = 525, EmbodiedEnergy = 30, EmbodiedCarbon = 3, Life = 30, DataSource = "EnergyPlus, RSMEANS, Citherlet et al" });


            #endregion


            #region ZoneDefTemplates

            Library.Add(new WindowSettings());

            var ZD_Residential = Library.Add(new ZoneDefinition() { Name = "Residential", Loads = ZL_Residential, Conditioning = ZC_Residential, DomHotWater = DHW_Residential, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Residential" });
            var ZD_Kitchen = Library.Add(new ZoneDefinition() { Name = "Kitchen", Loads = ZL_Kitchen, Conditioning = ZC_Kitchen, DomHotWater = DHW_Kitchen, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Residential" });
            var ZD_Office = Library.Add(new ZoneDefinition() { Name = "Office", Loads = ZL_Office, Conditioning = ZC_Office, DomHotWater = DHW_Office, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Office" });
            var ZD_MeetingRoom = Library.Add(new ZoneDefinition() { Name = "MeetingRoom", Loads = ZL_MeetingRoom, Conditioning = ZC_MeetingRoom, DomHotWater = DHW_MeetingRoom, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Office" });
            var ZD_HotelRoom = Library.Add(new ZoneDefinition() { Name = "HotelRoom", Loads = ZL_HotelRoom, Conditioning = ZC_HotelRoom, DomHotWater = DHW_HotelRoom, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Residential" });
            var ZD_ClassRoom = Library.Add(new ZoneDefinition() { Name = "ClassRoom", Loads = ZL_ClassRoom, Conditioning = ZC_ClassRoom, DomHotWater = DHW_ClassRoom, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Education" });
            var ZD_Library = Library.Add(new ZoneDefinition() { Name = "Library", Loads = ZL_Library, Conditioning = ZC_Library, DomHotWater = DHW_Library, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Education" });
            var ZD_LectureHall = Library.Add(new ZoneDefinition() { Name = "LectureHall", Loads = ZL_LectureHall, Conditioning = ZC_LectureHall, DomHotWater = DHW_LectureHall, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Education" });
            var ZD_SuperMarket = Library.Add(new ZoneDefinition() { Name = "SuperMarket", Loads = ZL_SuperMarket, Conditioning = ZC_SuperMarket, DomHotWater = DHW_SuperMarket, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Commercial" });
            var ZD_Shopping = Library.Add(new ZoneDefinition() { Name = "Shopping", Loads = ZL_Shopping, Conditioning = ZC_Shopping, DomHotWater = DHW_Shopping, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Commercial" });
            var ZD_ScienceLab = Library.Add(new ZoneDefinition() { Name = "ScienceLab", Loads = ZL_ScienceLab, Conditioning = ZC_ScienceLab, DomHotWater = DHW_ScienceLab, Ventilation = ZV_ModerateAirTightness, Materials = UVal2_Mass, Category = "Education" });











            var fl1 = Library.Add(new FloorDefinition() { BuildingID = "Default", Type = "INT", Name = "Default_INT" });
            var fl2 = Library.Add(new FloorDefinition() { BuildingID = "Default", Type = "B", Name = "Default_B" });
            var fl3 = Library.Add(new FloorDefinition() { BuildingID = "Default", Type = "G", Name = "Default_G" });
            var fl4 = Library.Add(new FloorDefinition() { BuildingID = "Default", Type = "R", Name = "Default_R" });



            BuildingDefinition b = new BuildingDefinition { Name = "Default" };
            b.Floors.Add(fl1);
            b.Floors.Add(fl2);
            b.Floors.Add(fl3);
            b.Floors.Add(fl4);

            Library.Add(b);


            #endregion


            return Library;
        }




    }
}
