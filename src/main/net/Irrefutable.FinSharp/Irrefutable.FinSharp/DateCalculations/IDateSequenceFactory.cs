using System;
using System.Collections.Generic;

namespace Irrefutable.FinSharp.DateCalculations
{
    public interface IDateSequenceFactory
    {
        IEnumerable<DateTime> CreateDateSequence(DateSpan sequenceRange,
            IHolidayCalendar holidayCalendar,
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL,
            DateSequenceGenerationRule sequenceGenerationRule = DateSequenceGenerationRule.FORWARD,
            BusinessDayConvention businessDayConvention = BusinessDayConvention.FOLLOWING); 
    }

    public enum BusinessDayConvention
    {
        NO_ADJUSTMENT = 0,
        FOLLOWING = 1,
        MODIFIED_FOLLOWING = 2,
        PRECEDING = 3,
        MODIFIED_PRECEDING = 4
    }

    public enum DateSequenceFrequency
    {
        NEVER = 0,
        ANNUAL = 1,
        SEMIANNUAL = 2,
        EVERY_FOURTH_MONTH = 3,
        QUARTERLY = 4,
        BIMONTHLY = 6,
        MONTHLY = 12,
        EVERY_FOURTH_WEEK = 13,
        BIWEEKLY = 26,
        WEEKLY = 52,
        DAILY = 365
    }


    public enum DateSequenceGenerationRule
    {
        FORWARD = 0,
        BACKWARD = 1
    }
}
