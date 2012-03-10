using System;
using System.Collections.Generic;

namespace Irrefutable.FinSharp.DateCalculations
{
    public interface IHolidayCalendar
    {
        IEnumerable<DateTime> Holidays { get; }
        IEnumerable<DayOfWeek> Weekend { get; }  
    }
}
