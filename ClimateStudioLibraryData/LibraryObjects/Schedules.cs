using CSEnergyLib.LibraryObjects;
using CSEnergyLib.Utilities;
using ProtoBuf;
using System;
 using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
 using System.Runtime.Serialization;

namespace CSEnergyLib.LibraryObjects
{
    [DataContract]
    [ProtoContract]
    public class CSDaySchedule : LibraryComponent
    {

        [DataMember][ProtoMember(1)] public ScheduleType Type { get; set; } = ScheduleType.Fraction;
        [DataMember] [ProtoMember(2)] public List<double> Values { get; set; } = new List<double>();
        [DataMember] [ProtoMember(3)] public double Total { get; set; }


        public CSDaySchedule() { }
        public CSDaySchedule(string name, ScheduleType type, List<double> vals)
        {
            Name = name;
            Type = type;
            Values = vals;
            Total = vals.Sum();
        }
        public void Update(string name, ScheduleType type, List<double> vals)
        {
            Name = name;
            Type = type;
            Values = vals;
        }
        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }
            if (this.Type == ScheduleType.Fraction)
            {
                for (int i = 0; i < this.Values.Count; i++)
                {
                    if (this.Values[i] < 0) { this.Values[i] = 0; changed = true; }
                    if (this.Values[i] > 1) { this.Values[i] = 1; changed = true; }
                }
            }
            return changed;
        }


        public static CSDaySchedule fromJSON(string json)
        {
            return Serialization.Deserialize<CSDaySchedule>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSDaySchedule>(this);
        }

    }
    [DataContract]
    [ProtoContract]
    public class WeekSchedule 
    {
        [DataMember] [ProtoMember(1, AsReference = true, OverwriteList = true)] public CSDaySchedule[] Days { get; set; } = new CSDaySchedule[7];
        [DataMember] [ProtoMember(2)] public DateTime From { get; set; } = new DateTime(2006, 1, 1);
        [DataMember] [ProtoMember(3)] public DateTime To { get; set; } = new DateTime(2006, 12,31);

        
        public WeekSchedule() { Days = new CSDaySchedule[7]; }
        public WeekSchedule(CSDaySchedule[] days, DateTime From , DateTime To)
        {
            Days = days;
        }
       



    }
    [DataContract]
    [ProtoContract]
    public class CSYearSchedule : LibraryComponent
    {

        [DataMember] [ProtoMember(1)] public ScheduleType Type { get; set; } = ScheduleType.Fraction;
        [DataMember] [ProtoMember(2, AsReference = true)] public List<WeekSchedule> WeekSchedules = new List<WeekSchedule>();
        [DataMember] [DefaultValue(0)] [ProtoMember(3)] public double Total { get; set; }

        public CSYearSchedule() { }
        public CSYearSchedule(string name, ScheduleType type, List<WeekSchedule> weekScheduleNames)  //, List<int> monthFrom, List<int> dayForm, List<int> monthTill, List<int> dayTill
        {
            Name = name;
            Type = type;
            WeekSchedules = weekScheduleNames;
            Total = this.LoadHours();
        }

 
        public bool Correct()
        {
            bool changed = false;

            string cleanName = CSFormatting.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }
            return changed;
        }
        public double[] To8760Array()
        {

            var arr = new double[8760];
            int hrcount = 0;

            for (int i = 0; i < this.WeekSchedules.Count; i++)
            {
                var StartDate = WeekSchedules[i].From;// new DateTime(2006,  this.MonthFrom[i], this.DayFrom[i]);
                var EndDate = WeekSchedules[i].To; //new DateTime(2006, this.MonthTill[i], this.DayTill[i]);

                var allDaysInPeriod = TimeHelpers.EachDay(StartDate, EndDate);

                foreach (DateTime day in allDaysInPeriod)
                {
                    var dofw = day.DayOfWeek;
                    int dofwInt = 0;

                    switch (dofw)
                    {
                        case (System.DayOfWeek.Monday):
                            dofwInt = 0;
                            break;
                        case (System.DayOfWeek.Tuesday):
                            dofwInt = 1;
                            break;
                        case (System.DayOfWeek.Wednesday):
                            dofwInt = 2;
                            break;
                        case (System.DayOfWeek.Thursday):
                            dofwInt = 3;
                            break;
                        case (System.DayOfWeek.Friday):
                            dofwInt = 4;
                            break;
                        case (System.DayOfWeek.Saturday):
                            dofwInt = 5;
                            break;
                        case (System.DayOfWeek.Sunday):
                            dofwInt = 6;
                            break;
                    }


                    for (int hr = 0; hr < 24; hr++)
                    {

                        if (hrcount > 8759) continue;

                        arr[hrcount] = this.WeekSchedules[i].Days[dofwInt].Values[hr];

                        hrcount++;
                    }
                }
            }



            return arr;

        }

        public double LoadHours() {

            var arr = this.To8760Array();
            return arr.Sum();
        }
        public static CSYearSchedule QuickSchedule(string Name, double[] dayArray, ScheduleType type , string category, string dataSource, ref CSLibrary Library)
        {

            CSDaySchedule someDaySchedule = new CSDaySchedule(Name, type, dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            someDaySchedule = Library.Add(someDaySchedule);
            CSDaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(daySchedulesArray, new DateTime(2006, 1,1), new DateTime(2006,12,31));
            //someWeekSchedule.DataSource = dataSource;
            //someWeekSchedule.Category = category;
            //someWeekSchedule = Library.Add(someWeekSchedule);
            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            CSYearSchedule someYearSchedule = new CSYearSchedule(Name, type, weekSchedulesArray.ToList()); //, MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList()
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;

            Library.Add(someYearSchedule);
            return someYearSchedule;

        }

        public static CSYearSchedule QuickSchedule(string Name, double[] dayArray, double[] weArray, ScheduleType type , string category, string dataSource, ref CSLibrary Library)
        {


            //int[] MonthFrom = { 1 };
            //int[] DayFrom = { 1 };
            //int[] MonthTo = { 12 };
            //int[] DayTo = { 31 };

            CSDaySchedule someDaySchedule = new CSDaySchedule(Name, type, dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            someDaySchedule = Library.Add(someDaySchedule);

            CSDaySchedule weSchedule = new CSDaySchedule(Name + "WeekEnd", type, weArray.ToList());
            weSchedule.DataSource = dataSource;
            weSchedule.Category = category;
            weSchedule = Library.Add(weSchedule);

            CSDaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, weSchedule, weSchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(daySchedulesArray , new DateTime(2006, 1, 1), new DateTime(2006, 12, 31));
            //someWeekSchedule.DataSource = dataSource;
            //someWeekSchedule.Category = category;
            //someWeekSchedule = Library.Add(someWeekSchedule);

            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            CSYearSchedule someYearSchedule = new CSYearSchedule(Name, type, weekSchedulesArray.ToList()); //, MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList()
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;

            Library.Add(someYearSchedule);
            return someYearSchedule;

        }

        public static CSYearSchedule fromJSON(string json)
        {
            return Serialization.Deserialize<CSYearSchedule>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSYearSchedule>(this);
        }

    }


    [DataContract]
    [ProtoContract]
    public class CSArraySchedule : LibraryComponent
    {
        [DataMember]
        [ProtoMember(1)]
        public ScheduleType Type { get; set; } = ScheduleType.Fraction;
        [DataMember]
        [ProtoMember(2)]
        public double[] Values;
        [DataMember][DefaultValue(0)]
        [ProtoMember(3)]
        public double Total { get; set; }
        public CSArraySchedule() { }
        public CSArraySchedule(double[] values8760, ScheduleType type) {

            Values = values8760;
            Type = type;
            Total = this.LoadHours();
        }

        public double[] To8760Array()
        {
            return Values;
        }
        public double LoadHours()
        {

            var arr = this.To8760Array();
            return arr.Sum();
        }

        public static CSArraySchedule fromJSON(string json)
        {
            return Serialization.Deserialize<CSArraySchedule>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<CSArraySchedule>(this);
        }
    }
}