using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.FinSharp.DateCalculations;

namespace StreamCipher.FinSharp.ServiceProxies
{
    public interface IHolidayCalendarSvc
    {
        IHolidayCalendar GetHolidayCalendar(String holidayCalendarRef);
    }
}
