using System;

namespace Irrefutable.FinSharp.DateCalculations
{
    public class DateSpan
    {
        public DateSpan(DateTime startDate, DateTime endDate)
        {
            DateTime startDt = startDate.Date;
            DateTime endDt = endDate.Date;
            if (endDt < startDt) throw new ArgumentException("End date cannot be lesser than start date");
            StartDate = startDt;
            EndDate = endDt;
            Days = (endDt - startDt).Days;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Days { get; set; }
    }

    //TODO: Document methods to be able to generate documentation.
    //TODO: Refactor ICountDays to use DateSpan.
    //TODO Implement IDateSequenceFactory and IHolidayCalendar.

}
