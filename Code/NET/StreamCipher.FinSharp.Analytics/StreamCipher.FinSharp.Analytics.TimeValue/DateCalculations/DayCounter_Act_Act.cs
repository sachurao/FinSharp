using StreamCipher.FinSharp.Analytics.Model.Dates;

namespace StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations
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
