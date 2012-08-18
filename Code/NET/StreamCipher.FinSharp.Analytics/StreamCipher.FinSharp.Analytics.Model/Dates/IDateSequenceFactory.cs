using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Analytics.Model.Dates
{
    public interface IDateSequenceFactory
    {
        IEnumerable<DateTime> CreateDateSequence(DateSpan sequenceRange,
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL,
            HolidayCalendar holidayCalendar = null,
            BusinessDayConvention businessDayConvention = BusinessDayConvention.NO_ADJUSTMENT);
    }

    
}
