using System;
using System.Collections.Generic;

namespace Irrefutable.FinSharp.DateCalculations
{
    public class DateSequenceFactory:IDateSequenceFactory
    {
        public IEnumerable<DateTime> CreateDateSequence(DateSpan sequenceRange, 
            IHolidayCalendar holidayCalendar, 
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL, 
            DateSequenceGenerationRule sequenceGenerationRule = DateSequenceGenerationRule.FORWARD, 
            BusinessDayConvention businessDayConvention = BusinessDayConvention.FOLLOWING)
        {
            var getNextDateInSequence = GetDateGeneratorFunction(frequency, sequenceGenerationRule);
            DateTime retVal = sequenceRange.StartDate;
            retVal = getNextDateInSequence(retVal).Date;
            while (retVal < sequenceRange.EndDate)
            {
                yield return retVal;
                retVal = getNextDateInSequence(retVal).Date;
            }       
        }

        private Func<DateTime, DateTime> GetDateGeneratorFunction(
            DateSequenceFrequency frequency, DateSequenceGenerationRule generationRule)
        {
            int factor = generationRule == DateSequenceGenerationRule.BACKWARD ? -1 : 1;
            Func<DateTime, DateTime> retVal = (dt) => dt;
            switch (frequency)
            {
                case DateSequenceFrequency.ANNUAL:
                    retVal = dt => dt.AddYears(1*factor);
                    break;
                case DateSequenceFrequency.SEMIANNUAL:
                    retVal = dt => dt.AddMonths(6*factor);
                    break;
                case DateSequenceFrequency.EVERY_FOURTH_MONTH:
                    retVal = dt => dt.AddMonths(4 * factor);
                    break;
                case DateSequenceFrequency.QUARTERLY:
                    retVal = dt => dt.AddMonths(3 * factor);
                    break;
                case DateSequenceFrequency.BIMONTHLY:
                    retVal = dt => dt.AddMonths(2 * factor);
                    break;
                case DateSequenceFrequency.MONTHLY:
                    retVal = dt => dt.AddMonths(1 * factor);
                    break;
                case DateSequenceFrequency.EVERY_FOURTH_WEEK:
                    retVal = dt => dt.AddDays(7*4 * factor);
                    break;
                case DateSequenceFrequency.BIWEEKLY:
                    retVal = dt => dt.AddDays(7*2 * factor);
                    break;
                case DateSequenceFrequency.WEEKLY:
                    retVal = dt => dt.AddDays(7* factor);
                    break;
                case DateSequenceFrequency.DAILY:
                    retVal = dt => dt.AddDays(factor);
                    break;
            }
            return retVal;
        }
    }
}
