using System;

namespace StreamCipher.FinSharp.DateCalculations.DayCounters
{
    public abstract class DayCounter_30_360_Base:ICountDays
    {
        protected int ComputeDaysCore(DateTime start, DateTime end)
        {
            return (end.Day - start.Day) + 30 * (end.Month - start.Month) + 360 * (end.Year - start.Year);
        }

        public int? NotionalDaysInYear
        {
            get { return 360; }
        }

        public int ComputeDaysBetweenDates(DateSpan dateSpan)
        {
            throw new NotImplementedException();
        }
    }
}
