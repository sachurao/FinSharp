using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using StreamCipher.FinSharp.DateCalculations;
using StreamCipher.FinSharp.DateCalculations.DayCounters;
using StreamCipher.FinSharp.InstrumentValuation.Core;
using StreamCipher.FinSharp.Instruments;
using StreamCipher.FinSharp.ServiceProxies;
using StreamCipher.Common.Ioc.DependencyResolution;
using NUnit.Framework;

namespace StreamCipher.FinSharp.InstrumentValuation.Tests.Core
{
    public class InterestAccrualCalculatorFixture
    {
        private readonly InstrumentFactory _instrumentFactory = new InstrumentFactory();
        private InterestAccrualCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new InterestAccrualCalculator();
            ServiceLocator.ClearServices();
            var resolver = new ProgrammaticDependencyManager();
            resolver.Register<IDayCounterFactory, DayCounterFactory>();
            resolver.Register<IHolidayCalendarSvc, HolidayCalendarSvcStub>();
            resolver.Register<IDateSequenceFactory, DateSequenceFactory>();
            ServiceLocator.Initialise(resolver);
        }

        

        [TestCase(BusinessDayConvention.NO_ADJUSTMENT, DateSequenceFrequency.SEMIANNUAL, DayCountConvention.DCC_ACT_ACT, 
            1995, 08, 15, 1995, 08, 15, 1996, 02, 15, 2025, 08, 15, 6.875, 2009, 04, 01, Result = 45)]
        [TestCase(BusinessDayConvention.FOLLOWING, DateSequenceFrequency.SEMIANNUAL, DayCountConvention.DCC_ACT_ACT,
            1995, 08, 15, 1995, 08, 15, 1996, 02, 15, 2025, 08, 15, 6.875, 2009, 04, 01, Result = 44)]
        [TestCase(BusinessDayConvention.PRECEDING, DateSequenceFrequency.SEMIANNUAL, DayCountConvention.DCC_ACT_ACT,
            1995, 08, 15, 1995, 08, 15, 1996, 02, 15, 2025, 08, 15, 6.875, 2009, 04, 01, Result = 47)]
        
        public int ComputeAccruedDays_WithFixedCouponWeekDayCalendarNoRecordDateOffset_Test(BusinessDayConvention bdc, 
            DateSequenceFrequency dsf,DayCountConvention dcc, int y1, int m1, int d1, int y2, int m2, int d2, 
            int y3, int m3, int d3, int y4, int m4, int d4, decimal cpnRate, int y5, int m5, int d5
            )
        {
            //Arrange
            var spec = new BondSpecification()
            {
                BusinessDayConvention = bdc,
                CouponFrequency = dsf,
                CouponYearBasis = dcc,
                HolidayCalendarRef = DefaultConstants.DEFAULT_HOLIDAY_CALENDAR_REF,
                IssueDate = new DateTime(y1, m1, d1),
                FirstAccrualDate = new DateTime(y2, m2, d2),
                FirstCouponDate = new DateTime(y3, m3, d3),
                RecordDateOffset = 0,
                RedemptionDate = new DateTime(y4, m4, d4)
            };
            spec.RateSchedule = _instrumentFactory.FixedCoupon(spec.FirstCouponDate, cpnRate);
            var bond = _instrumentFactory.CreateBond(spec);

            //Act
            return _calculator.ComputeAccruedDays(bond, new DateTime(y5, m5, d5));
        }

    }

    class HolidayCalendarSvcStub : IHolidayCalendarSvc
    {
        public IHolidayCalendar GetHolidayCalendar(string holidayCalendarRef)
        {
            return new WeekDayCalendar();
        }
    }
}
