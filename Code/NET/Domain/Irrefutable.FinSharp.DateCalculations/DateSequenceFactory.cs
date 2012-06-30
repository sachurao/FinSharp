using System;
using System.Collections.Generic;
using System.Linq;
using Irrefutable.FinSharp.ServiceProxies;
using Irrefutable.Shared.Ioc.DependencyResolution;

namespace Irrefutable.FinSharp.DateCalculations
{
    public class DateSequenceFactory:IDateSequenceFactory
    {
        public IEnumerable<DateTime> CreateDateSequence(DateSpan sequenceRange, 
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL)
        {
            var getNextDateInSequence = GetDateGeneratorFunction(frequency);
            DateTime retVal = sequenceRange.StartDate;
            while (retVal < sequenceRange.EndDate)
            {
                yield return retVal;
                retVal = getNextDateInSequence(retVal);
            }       
        }

        public IEnumerable<DateTime> CreateBusinessDateSequence(IEnumerable<DateTime> notionalDates, 
            string holidayCalendarRef, 
            BusinessDayConvention businessDayConvention = BusinessDayConvention.NO_ADJUSTMENT)
        {
            //First get holidays...
            var calendarSvc = ServiceLocator.GetImplementationOf<IHolidayCalendarSvc>();
            IHolidayCalendar holidayCalendar = calendarSvc.GetHolidayCalendar(holidayCalendarRef);
            var holidays = holidayCalendar.Holidays.ToList();
            var weekendDays = holidayCalendar.Weekend.ToList();

            var businessDayRoller = GetBusinessDayRoller(holidays, weekendDays, businessDayConvention);

            foreach (var notionalDate in notionalDates)
            {
                DateTime retVal = notionalDate;
                if (holidays.Contains(retVal) || weekendDays.Contains(retVal.DayOfWeek))
                {
                    //We now have to adjust it to get the right business day.
                    retVal = businessDayRoller(retVal);
                }
                yield return retVal;
            }
        }

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

        private Func<DateTime,DateTime> GetBusinessDayRoller(IList<DateTime> holidays,
            IList<DayOfWeek> weekendDays,
            BusinessDayConvention businessDayConvention)
        {
            Func<DateTime, DateTime> retVal = (dt) => dt;
            switch (businessDayConvention)
            {
                case BusinessDayConvention.FOLLOWING:
                    retVal = (dt) =>
                                 {
                                     var businessDate = dt;
                                     while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                                     {
                                         businessDate = businessDate.AddDays(1);
                                     }
                                     return businessDate;
                                 };
                    break;
                case BusinessDayConvention.MODIFIED_FOLLOWING:
                    retVal = (dt) =>
                                 {
                                     var businessDate = dt;
                                     while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                                     {
                                         businessDate = businessDate.AddDays(1);
                                     }
                                     //If the month has changed
                                     if (businessDate.Month != dt.Month)
                                     {
                                         //Using the previous business date instead
                                         businessDate = dt;
                                         while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                                         {
                                             businessDate = businessDate.AddDays(-1);
                                         }
                                     
                                     }
                                     return businessDate;
                                 };
                    break;
                case BusinessDayConvention.PRECEDING:
                    retVal = (dt) =>
                    {
                        var businessDate = dt;
                        while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                        {
                            businessDate = businessDate.AddDays(-1);
                        }
                        return businessDate;
                    };
                    break;
                case BusinessDayConvention.MODIFIED_PRECEDING:
                    retVal = (dt) =>
                                 {
                                     var businessDate = dt;
                                     while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                                     {
                                         businessDate = businessDate.AddDays(-1);
                                     }
                                     //If the month has changed
                                     if (businessDate.Month != dt.Month)
                                     {
                                         //Using the previous business date instead
                                         businessDate = dt;
                                         while (holidays.Contains(businessDate) || weekendDays.Contains(businessDate.DayOfWeek))
                                         {
                                             businessDate = businessDate.AddDays(1);
                                         }
                                     
                                     }
                                     return businessDate;
                                 };
                    break;
                default:
                    retVal = (dt) => dt;
                    break;
            }
            return retVal;
        }
    }
}
