using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.FinSharp.Analytics.Model.Dates;
using StreamCipher.FinSharp.Analytics.Model.Schedules;
using StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations;

namespace StreamCipher.FinSharp.Analytics.TimeValue.Schedules
{
    public class ScheduleFactory
    {
        public DecimalSchedule CreateFixedRateSchedule(
            DateSpan term, Decimal decimalValue,
            DateSequenceFrequency frequency = DateSequenceFrequency.ANNUAL,
            HolidayCalendar holidayCalendar = null,
            BusinessDayConvention businessDayConvention = BusinessDayConvention.NO_ADJUSTMENT)
        {
            IDateSequenceFactory sequenceFactory = new DateSequenceFactory();
            IEnumerable<DateTime> dateSequence = sequenceFactory.CreateDateSequence(
                term, frequency, holidayCalendar, businessDayConvention);

            DecimalSchedule retVal = null;

            if (dateSequence!=null && dateSequence.Any())
            {
                retVal = new DecimalSchedule();
                foreach (var dateTime in dateSequence)
                {
                    retVal.Add(dateTime, decimalValue);
                }
            }
            return retVal;
        }

        public DecimalSchedule CreatePaymentSchedule(Decimal face,
            DecimalSchedule fixedRateSchedule, DayCountConvention dcc)
        {
            ICountDays dayCounter = DayCounterFactory.Create(dcc);
            DecimalSchedule retVal = null;
            if (fixedRateSchedule!=null)
            {
                DateTime? startDate;
                Decimal? applicableRate;
                foreach (var dateDecimalPair in fixedRateSchedule)
                {
                    if (applicableRate.HasValue && startDate.HasValue)
                    {
                        
                    }
                    startDate = dateDecimalPair.Key.Date;
                    applicableRate = dateDecimalPair.Value;
                }
            }
        }
    }
}
