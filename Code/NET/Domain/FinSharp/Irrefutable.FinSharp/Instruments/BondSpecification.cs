using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.FinSharp.DateCalculations;

namespace Irrefutable.FinSharp.Instruments
{
    /// <summary>
    /// Represents a standard fixed-income instrument.
    /// </summary>
    public class BondSpecification
    {
        public IDictionary<DateTime, Decimal> RateSchedule { get; set; }
        public DateSequenceFrequency CouponFrequency { get; set; }
        public DayCountConvention CouponYearBasis { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime FirstAccrualDate { get; set; }
        public DateTime FirstCouponDate { get; set; }
        public int RecordDateOffset { get; set; }
        public DateTime RedemptionDate { get; set; }
        public Decimal RedemptionValue { get; set; }
        public String HolidayCalendarRef { get; set; }
        public BusinessDayConvention BusinessDayConvention { get; set; }
    }
}
