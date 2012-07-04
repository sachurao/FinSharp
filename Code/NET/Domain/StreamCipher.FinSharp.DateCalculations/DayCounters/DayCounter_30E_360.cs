using System;

namespace StreamCipher.FinSharp.DateCalculations.DayCounters
{
    /// <summary>
    /// Represents 30E/360 calculations.
    /// Implements <see cref="ICountDays"/>
    /// </summary>
    public class DayCounter_30E_360:DayCounter_30_360_Base
    {
        /// <summary>
        /// If StartDate is 31, StartDate is set to 30.
        /// If EndDate is 31, EndDate is set to 30.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int ComputeDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            var start = startDate;
            var end = endDate;
            if (startDate.Day == 31)
                start = new DateTime(startDate.Year, startDate.Month, 30);
            if (endDate.Day == 31)
                end = new DateTime(endDate.Year, endDate.Month, 30);

            return ComputeDaysCore(start, end);
        }
    }
}
