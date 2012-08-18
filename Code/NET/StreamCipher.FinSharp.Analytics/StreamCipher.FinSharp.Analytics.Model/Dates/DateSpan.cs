using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Analytics.Model.Dates
{
    /// <summary>
    /// Represents the date interval between two dates.
    /// </summary>
    public class DateSpan
    {
        public DateSpan(DateTime startDate, DateTime endDate)
        {
            DateTime startDt = startDate.Date;
            DateTime endDt = endDate.Date;
            if (endDt <= startDt) throw new ArgumentException("End date cannot be lesser than start date");
            StartDate = startDt;
            EndDate = endDt;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int Days { get { return (EndDate - StartDate).Days; } }
    }
}
