using System;
using StreamCipher.FinSharp.Analytics.Model.Dates;

namespace StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations
{
    /// <summary>
    /// Represents 30/360 date calculations.
    /// Implements <see cref="ICountDays"/>
    /// </summary>    
    public class DayCounter_30_360:DayCounter_30_360_Base
    {
        /// <summary>
        /// If StartDate is 31, it is reset to 30
        /// If EndDate is 31, it is reset to 30 only if StartDate is 30 or 31.
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
            if (endDate.Day == 31 && start.Day >= 30)
                end = new DateTime(endDate.Year, endDate.Month, 30);

            return ComputeDaysCore(start, end);
        }

        
    }
}
