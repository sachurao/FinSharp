namespace Irrefutable.FinSharp.DateCalculations.DayCounters
{
    public class DayCounter_Act_360:DayCounter_Act_Act
    {
        public override int? NotionalDaysInYear
        {
            get { return 360; }
        }
    }
}
