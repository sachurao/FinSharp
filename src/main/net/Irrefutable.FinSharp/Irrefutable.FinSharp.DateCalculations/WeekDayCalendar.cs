using System;
using System.Collections.Generic;
using System.Linq;

namespace Irrefutable.FinSharp.DateCalculations
{
    public class WeekDayCalendar:IHolidayCalendar
    {
        public IEnumerable<DateTime> Holidays
        {
            get { return Enumerable.Empty<DateTime>(); }
        }

        public IEnumerable<DayOfWeek> Weekend
        {
            get { return new List<DayOfWeek>() {DayOfWeek.Saturday, DayOfWeek.Sunday}; }
        }
    }
}
