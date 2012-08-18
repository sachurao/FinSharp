using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.FinSharp.Analytics.Model.Dates;

namespace StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations
{
    public class DateSequenceFactory:IDateSequenceFactory
    {
        #region Implementation of IDateSequenceFactory

        public IEnumerable<DateTime> CreateDateSequence(DateSpan sequenceRange, 
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL,
            HolidayCalendar holidayCalendar = null,
            BusinessDayConvention businessDayConvention = BusinessDayConvention.NO_ADJUSTMENT)
        {
            var getNextDateInSequence = GetDateGeneratorFunction(frequency);
            var businessDayRoller = GetBusinessDayRollFunction(businessDayConvention);
            DateTime retVal = sequenceRange.StartDate;

            while (retVal <= sequenceRange.EndDate)
            {
                yield return retVal;
                retVal = businessDayRoller(getNextDateInSequence(retVal),holidayCalendar);
            }
        }

        #endregion

        #region Private Methods

        private Func<DateTime, DateTime> GetDateGeneratorFunction(
            DateSequenceFrequency frequency)
        {
            Func<DateTime, DateTime> retVal = (dt) => dt;
            switch (frequency)
            {
                case DateSequenceFrequency.ANNUAL:
                    retVal = dt => dt.AddYears(1).Date;
                    break;
                case DateSequenceFrequency.SEMIANNUAL:
                    retVal = dt => dt.AddMonths(6).Date;
                    break;
                case DateSequenceFrequency.EVERY_FOURTH_MONTH:
                    retVal = dt => dt.AddMonths(4).Date;
                    break;
                case DateSequenceFrequency.QUARTERLY:
                    retVal = dt => dt.AddMonths(3).Date;
                    break;
                case DateSequenceFrequency.BIMONTHLY:
                    retVal = dt => dt.AddMonths(2).Date;
                    break;
                case DateSequenceFrequency.MONTHLY:
                    retVal = dt => dt.AddMonths(1).Date;
                    break;
                case DateSequenceFrequency.EVERY_FOURTH_WEEK:
                    retVal = dt => dt.AddDays(7 * 4).Date;
                    break;
                case DateSequenceFrequency.BIWEEKLY:
                    retVal = dt => dt.AddDays(7 * 2).Date;
                    break;
                case DateSequenceFrequency.WEEKLY:
                    retVal = dt => dt.AddDays(7).Date;
                    break;
                case DateSequenceFrequency.DAILY:
                    retVal = dt => dt.AddDays(1).Date;
                    break;
            }
            return retVal;
        }

        private Func<DateTime, HolidayCalendar, DateTime> GetBusinessDayRollFunction(
            BusinessDayConvention businessDayConvention)
        {
            Func<DateTime, HolidayCalendar,  DateTime> retVal = (dt,calendar) => dt;
            switch (businessDayConvention)
            {
                case BusinessDayConvention.FOLLOWING:
                    retVal = (dt, calendar) =>
                    {
                        var businessDate = dt;
                        if (calendar != null)
                        {
                            while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                            {
                                businessDate = businessDate.AddDays(1);
                            }
                        }
                        return businessDate;
                    };
                    break;
                case BusinessDayConvention.MODIFIED_FOLLOWING:
                    retVal = (dt, calendar) =>
                    {
                        var businessDate = dt;

                        if (calendar!=null)
                        {
                            while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                            {
                                businessDate = businessDate.AddDays(1);
                            }
                            //If the month has changed
                            if (businessDate.Month != dt.Month)
                            {
                                //Using the previous business date instead
                                businessDate = dt;
                                while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                                {
                                    businessDate = businessDate.AddDays(-1);
                                }

                            }
                        }
                        return businessDate;
                    };
                    break;
                case BusinessDayConvention.PRECEDING:
                    retVal = (dt, calendar) =>
                    {
                        var businessDate = dt;
                        if (calendar!=null)
                        {
                            while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                            {
                                businessDate = businessDate.AddDays(-1);
                            }
                        }
                        return businessDate;
                    };
                    break;
                case BusinessDayConvention.MODIFIED_PRECEDING:
                    retVal = (dt, calendar) =>
                    {
                        var businessDate = dt;
                        if (calendar!=null)
                        {
                            while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                            {
                                businessDate = businessDate.AddDays(-1);
                            }
                            //If the month has changed
                            if (businessDate.Month != dt.Month)
                            {
                                //Using the original business date instead
                                businessDate = dt;
                                while (calendar.Holidays.Contains(businessDate) || calendar.Weekend.Contains(businessDate.DayOfWeek))
                                {
                                    businessDate = businessDate.AddDays(1);
                                }

                            }
                        }
                        return businessDate;
                    };
                    break;
                default:
                    retVal = (dt, calendar) => dt;
                    break;
            }
            return retVal;
        }

        #endregion
    }
}
