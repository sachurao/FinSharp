using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.FinSharp.DateCalculations;

namespace Irrefutable.FinSharp.ServiceProxies
{
    public interface IHolidayCalendarSvc
    {
        IHolidayCalendar GetHolidayCalendar(String holidayCalendarRef);
    }
}
