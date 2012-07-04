using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Instruments.Bond
{
    /// <summary>
    /// Represents a bond instrument constructed from the BondSpecification passed in.
    /// </summary>
    public class BondInstrument
    {
        //Please use InstrumentFactory to create a new bond.
        internal BondInstrument(BondSpecification specification,
            IEnumerable<DateTime> notionalCouponDates = null, 
            CouponPaymentSchedule couponPayments = null
            )
        {
            Specification = specification;
            CouponRates = new InterestRateSchedule();
            foreach (var entry in specification.RateSchedule)
            {
                CouponRates.Add(entry.Key, entry.Value);
            }
            NotionalCouponDates = notionalCouponDates;
            CouponPayments = couponPayments;
        }

        public BondSpecification Specification { get; private set; }

        public IEnumerable<DateTime> NotionalCouponDates { get; private set; } 
        
        public CouponPaymentSchedule CouponPayments { get; private set; }

        public InterestRateSchedule CouponRates { get; private set; }

    }
}
