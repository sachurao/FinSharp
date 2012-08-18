namespace StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations
{
    public class DayCounter_Act_365 : DayCounter_Act_Act
    {
        public override int? NotionalDaysInYear
        {
            get { return 365; }
        }
    }
}
