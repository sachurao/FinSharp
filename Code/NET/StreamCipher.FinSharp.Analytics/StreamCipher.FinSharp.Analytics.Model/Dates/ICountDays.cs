using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Analytics.Model.Dates
{
    /// <summary>
    /// Represents year-basis calculations.
    /// </summary>
    public interface ICountDays
    {
        int ComputeDaysBetweenDates(DateSpan dateSpan);
        int? NotionalDaysInYear { get; }
    }

}
