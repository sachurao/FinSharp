using System;
using System.Collections.Generic;

namespace StreamCipher.FinSharp.DateCalculations
{
    public interface IHolidayCalendar
    {

        IEnumerable<DateTime> Holidays { get; }
        IEnumerable<DayOfWeek> Weekend { get; }  
    }
}
