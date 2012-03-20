﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.FinSharp.DateCalculations;
using Irrefutable.FinSharp.Instruments;
using Irrefutable.FinSharp.Instruments.Bond;
using Irrefutable.Shared.Ioc.DependencyResolution;

namespace Irrefutable.FinSharp.InstrumentValuation.Core
{
    public class InterestAccrualCalculator
    {
        
        public int ComputeAccruedDays(BondInstrument bond, DateTime settlementDate)
        {
            //Given a bond, find the active coupon period
            DateTime settleDate = settlementDate.Date;
            var prevCpnPaymentDate = (from p in bond.CouponPayments.Keys
                                          where p.Date <= settleDate
                                          select p).Max();
            IDayCounterFactory dayCounterFactory = ServiceLocator.GetImplementationOf<IDayCounterFactory>();
            var accrualDayCounter = dayCounterFactory.Create(bond.Specification.CouponYearBasis);
            return accrualDayCounter.ComputeDaysBetweenDates(new DateSpan(prevCpnPaymentDate, settleDate));
        }
    }
}
