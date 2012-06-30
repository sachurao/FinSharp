namespace Irrefutable.FinSharp.DateCalculations.DayCounters
{
	public class DayCounter_Act_Act:ICountDays
	{
        public int ComputeDaysBetweenDates(DateSpan dateSpan)
        {
            return (dateSpan.EndDate-dateSpan.StartDate).Days;
        }

        public virtual int? NotionalDaysInYear
        {
            get { return null; }
        }
    }
}
