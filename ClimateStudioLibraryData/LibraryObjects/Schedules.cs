using ArchsimLib.LibraryObjects;
using ArchsimLib.Utilities;
using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Runtime.Serialization;

namespace ArchsimLib.LibraryObjects
{
    [DataContract(IsReference = true)]
    public class DaySchedule : LibraryComponent
    {

        [DataMember]
        public ScheduleType Type { get; set; } = ScheduleType.Fraction;


        [DataMember]
        public List<double> Values { get; set; } = new List<double>();

        public DaySchedule() { }
        public DaySchedule(string name, ScheduleType type, List<double> vals)
        {
            Name = name;
            Type = type;
            Values = vals;
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

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
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




    }
    [DataContract(IsReference = true)]
    public class WeekSchedule 
    {

        [DataMember]
        public DaySchedule[] Days = new DaySchedule[7];


        [DataMember]
        public DateTime From { get; set; } = new DateTime(2006, 1, 1);
        [DataMember]
        public DateTime To { get; set; } = new DateTime(2006, 12,31);

        //[DataMember]
        //public int[] FromTo = { 1, 1, 12, 31 };

        public WeekSchedule() { }
        public WeekSchedule(DaySchedule[] days, DateTime From , DateTime To)
        {
            Days = days;
        }
        //public void Update(string name, DaySchedule[] days)
        //{
        //    Days = days;
        //}
        //public bool Correct()
        //{
        //    bool changed = false;


        //    // clean up all refereced names
        //    for (int i = 0; i < Days.Length; i++)
        //    {
        //        string cleanref = Formating.RemoveSpecialCharactersNotStrict(Days[i].Name);
        //        if (Days[i].Name != cleanref) { Days[i].Name = cleanref; changed = true; }
        //    }

        //    return changed;
        //}



    }
    [DataContract]
    public class YearSchedule : LibraryComponent
    {

        [DataMember]
        public ScheduleType Type { get; set; } = ScheduleType.Fraction;
        [DataMember]
        public List<WeekSchedule> WeekSchedules = new List<WeekSchedule>();
        //[DataMember]
        //public List<int> MonthFrom = new List<int>();
        //[DataMember]
        //public List<int> DayFrom = new List<int>();
        //[DataMember]
        //public List<int> MonthTill = new List<int>();
        //[DataMember]
        //public List<int> DayTill = new List<int>();


        public YearSchedule() { }
        public YearSchedule(string name, ScheduleType type, List<WeekSchedule> weekScheduleNames)  //, List<int> monthFrom, List<int> dayForm, List<int> monthTill, List<int> dayTill
        {
            Name = name;
            Type = type;
            WeekSchedules = weekScheduleNames;
            //MonthFrom = monthFrom;
            //DayFrom = dayForm;
            //MonthTill = monthTill;
            //DayTill = dayTill;

            //for (int i = 0; i < WeekSchedules.Count; i++)
            //{

            //    //if (MonthFrom.Count < WeekSchedules.Count) MonthFrom.Add(0);
            //    //if (DayFrom.Count < WeekSchedules.Count) DayFrom.Add(0);
            //    //if (MonthTill.Count < WeekSchedules.Count) MonthTill.Add(0);
            //    //if (DayTill.Count < WeekSchedules.Count) DayTill.Add(0);


            //}


        }

        public void Update(string name, ScheduleType type, List<WeekSchedule> weekScheduleNames) //, List<int> monthFrom, List<int> dayForm, List<int> monthTill, List<int> dayTill
        {

            Name = name;
            Type = type;
            WeekSchedules = weekScheduleNames;
            //MonthFrom = monthFrom;
            //DayFrom = dayForm;
            //MonthTill = monthTill;
            //DayTill = dayTill;

            for (int i = 0; i < WeekSchedules.Count; i++)
            {

                //if (MonthFrom.Count < WeekSchedules.Count) MonthFrom.Add(0);
                //if (DayFrom.Count < WeekSchedules.Count) DayFrom.Add(0);
                //if (MonthTill.Count < WeekSchedules.Count) MonthTill.Add(0);
                //if (DayTill.Count < WeekSchedules.Count) DayTill.Add(0);


            }


        }
        public bool Correct()
        {
            bool changed = false;

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }


            // clean up all refereced names
            //for (int i = 0; i < WeekSchedules.Count; i++)
            //{
            //    string cleanref = Formating.RemoveSpecialCharactersNotStrict(WeekSchedules[i].Name);
            //    if (WeekSchedules[i].Name != cleanref) { WeekSchedules[i].Name = cleanref; changed = true; }
            //}


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


        public static YearSchedule QuickSchedule(string Name, double[] dayArray, ScheduleType type , string category, string dataSource, ref Library Library)
        {

            DaySchedule someDaySchedule = new DaySchedule(Name, type, dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            someDaySchedule = Library.Add(someDaySchedule);
            DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(daySchedulesArray, new DateTime(2006, 1,1), new DateTime(2006,12,31));
            //someWeekSchedule.DataSource = dataSource;
            //someWeekSchedule.Category = category;
            //someWeekSchedule = Library.Add(someWeekSchedule);
            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            YearSchedule someYearSchedule = new YearSchedule(Name, type, weekSchedulesArray.ToList()); //, MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList()
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;

            Library.Add(someYearSchedule);
            return someYearSchedule;

        }

        public static YearSchedule QuickSchedule(string Name, double[] dayArray, double[] weArray, ScheduleType type , string category, string dataSource, ref Library Library)
        {


            //int[] MonthFrom = { 1 };
            //int[] DayFrom = { 1 };
            //int[] MonthTo = { 12 };
            //int[] DayTo = { 31 };

            DaySchedule someDaySchedule = new DaySchedule(Name, type, dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            someDaySchedule = Library.Add(someDaySchedule);

            DaySchedule weSchedule = new DaySchedule(Name + "WeekEnd", type, weArray.ToList());
            weSchedule.DataSource = dataSource;
            weSchedule.Category = category;
            weSchedule = Library.Add(weSchedule);

            DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, weSchedule, weSchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(daySchedulesArray , new DateTime(2006, 1, 1), new DateTime(2006, 12, 31));
            //someWeekSchedule.DataSource = dataSource;
            //someWeekSchedule.Category = category;
            //someWeekSchedule = Library.Add(someWeekSchedule);

            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            YearSchedule someYearSchedule = new YearSchedule(Name, type, weekSchedulesArray.ToList()); //, MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList()
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;

            Library.Add(someYearSchedule);
            return someYearSchedule;

        }
    }


    [DataContract]
    public class ScheduleArray : LibraryComponent
    {
        [DataMember]
        public ScheduleType Type { get; set; } = ScheduleType.Fraction;
        [DataMember]
        public double[] Values;

        public ScheduleArray() { }

        public double[] To8760Array()
        {
            return Values;
        }
    }
}