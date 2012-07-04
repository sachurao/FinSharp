namespace StreamCipher.FinSharp.DateCalculations
{
    /// <summary>
    /// Represents year-basis calculations
    /// </summary>
    public interface ICountDays
    {
        int ComputeDaysBetweenDates(DateSpan dateSpan);
        int? NotionalDaysInYear { get; }
    }
}
