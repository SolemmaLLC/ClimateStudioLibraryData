using System;
using System.Collections.Generic;

namespace ArchsimLib.Utilities
{

    public static class TimeHelpers
    {

        public static readonly int DaysInYear = 364;
        public static readonly int WeeksInYear = 52;
        public static readonly int[] DaysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        public static readonly string[] MonthsShort = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };
        public static readonly string[] Months = new string[]
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };


        /// <summary> returns hour-of-year index from M/D/H indices
        /// </summary>
        /// <param name="month">month index (0-11)</param>
        /// <param name="day">day index (0-30)</param>
        /// <param name="hour">hour index (0-23)</param>
        /// <returns></returns>
        public static int HourInYear(int monthIndex, int dayIndex, int hourIndex)
            {
                int hr = 0;
                for (int m = 0; m < 12; m++)
                {
                    if (m < monthIndex) hr += DaysInMonth[m] * 24;
                    else
                    {
                        hr += dayIndex * 24 + hourIndex;
                        break;
                    }
                }
                return hr;
            }

            public static void DayOfYear_To_MonthAndDay(int dayOfYear, out int month, out int day)
            {
                month = 0;
                day = 0;
                int doy = 0;
                for (int m = 0; m < 12; m++)
                {
                    if (doy + DaysInMonth[m] > dayOfYear)
                    {
                        month = m;
                        day = dayOfYear - doy;
                        return;
                    }
                    doy += DaysInMonth[m];
                }
            }
        
  


        static  public double[][] To24HourArrays(double[] arrIn )
        {
            var arr = new double[365][];
            int daycount = 0;
            int hrcnt = 0;
            for (int mo = 0; mo < 12; mo++)
            {
                for (int day = 0; day < TimeHelpers.DaysInMonth[mo]; day++)
                {
                    arr[daycount] = new double[24];
                    for (int hr = 0; hr < 24; hr++)
                    {
                        arr[daycount][hr] = arrIn[hrcnt];
                        hrcnt++;
                    }
                    daycount++;
                }
            }
            return arr;
        }



        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<DateTime> OneDayPerWeek(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(7))
                yield return day;
        }




        /// <summary> default meteorological year for sun position calculations and DateTime arithmetic 
        /// (2006 = non-leap year with Jan 1 falling on Sunday)
        /// </summary>
        public static int DefaultYear
        {
            get { return 2006; }
        }

 


        #region Methods

        /// <summary> gets decimal hour from DateTime
        /// </summary>
        /// <param name="datetime">datetime</param>
        /// <returns>hours from midnight as decimal</returns>
        public static double GetDecimalHour(DateTime datetime)
        {
            // get timespan from start of day in decimal hours
            TimeSpan ts = datetime - new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
            return ts.TotalHours;
        }

        /// <summary> converts (month, day, decimal_hour) to DateTime struct
        /// </summary>
        /// <param name="datetime">datetime</param>
        /// <param name="month">month [1, 12]</param>
        /// <param name="day">day [1, DaysInMonth]</param>
        /// <param name="hour">decimal hour [0.0, 24.0)</param>
        /// <returns>true if success, false if failure</returns>
        public static bool GetDateTime(out DateTime datetime, int month, int day, double hour)
        {
            // add decimal hours to midnight of yr/mo/day
            try
            {
                var midnight = new DateTime(DefaultYear, month, day, 0, 0, 0);
                datetime = midnight.AddHours(hour);
            }
            catch (Exception e)
            {
                datetime = new DateTime();
                Console.Error.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary> increments one hour, updating month/day/hour values
        /// </summary>
        /// <param name="month">month [0,11] </param>
        /// <param name="day">day [0,daysinmonth]</param>
        /// <param name="hour">hour[1,24]</param>
        public static void IncrementHour(ref int month, ref int day, ref int hour)
        {
            // increment
            month = (month == 11) ? 0 : month + 1;
            day = (day == DaysInMonth[month] - 1) ? 0 : day + 1;
            hour = (hour == 24) ? 1 : hour + 1;
        }

        /// <summary> returns DateTime from float hour-of-year value [0.0, 8760.0)
        /// </summary>
        /// <param name="hr_of_yr">hours since start of year [0.0, 8760.0)</param>
        /// <returns></returns>
        public static DateTime HOY_to_DateTime(float hr_of_yr)
        {
            var datetime = new DateTime();

            // get truncated hour as index
            int hoy = (int)Math.Truncate(hr_of_yr);

            // get partial hour as float
            float partial_hour = hr_of_yr - hoy;

            // check input validity
            if (hoy < 0 || hoy > 8759) return datetime;

            // loop through months
            int month = 0, day = 0, hour = 0;
            int start_of_next_mo = 0;
            for (int i = 0; i < 12; i++)
            {
                int start_of_mo = start_of_next_mo;
                start_of_next_mo += 24 * DaysInMonth[i];
                if (hoy < start_of_next_mo)
                {
                    // get month, day, hour index
                    month = i;
                    day = (int)Math.Truncate(((double)hoy - start_of_mo) / 24);
                    hour = (hoy - start_of_mo) - day * 24;

                    // get datetie
                    GetDateTime(out datetime, month + 1, day + 1, hour + partial_hour);
                    break;
                }
            }

            // return datetime
            return datetime;
        }


        #endregion


    }
}
