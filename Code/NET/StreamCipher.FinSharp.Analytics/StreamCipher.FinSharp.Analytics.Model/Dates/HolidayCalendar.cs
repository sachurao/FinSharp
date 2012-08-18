using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Analytics.Model.Dates
{
    public class HolidayCalendar
    {
        public IEnumerable<DateTime> Holidays { get; set; }
        public IEnumerable<DayOfWeek> Weekend { get; set; }

        public static HolidayCalendar CreateWeekendOnlyCalendar()
        {
            return new HolidayCalendar()
            {
                Holidays = Enumerable.Empty<DateTime>(),
                Weekend = new List<DayOfWeek>() {DayOfWeek.Saturday, DayOfWeek.Sunday}
            };
        }
    }
}
