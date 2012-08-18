using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Analytics.Model.Dates
{
    public class DatesConstants
    {
        public const string HOLIDAY_REF_WEEKEND_ONLY = "WeekendOnly";
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

    public enum DayCountConvention
    {
        DCC_ACT_ACT = 0,
        DCC_ACT_365 = 1,
        DCC_ACT_360 = 2,
        DCC_30_360 = 3,
        DCC_30E_360 = 4
    }

}
