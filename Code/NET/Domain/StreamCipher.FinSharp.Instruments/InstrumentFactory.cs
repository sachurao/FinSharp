using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.FinSharp.DateCalculations;
using StreamCipher.FinSharp.Instruments.Bond;
using StreamCipher.FinSharp.ServiceProxies;
using StreamCipher.Common.Ioc.DependencyResolution;

namespace StreamCipher.FinSharp.Instruments
{
    /// <summary>
    /// Factory class for instruments.
    /// </summary>
    public class InstrumentFactory
    {
        public BondInstrument CreateBond(BondSpecification specification, 
            IEnumerable<DateTime> notionalCouponSchedule = null, 
            CouponPaymentSchedule couponPayments = null)
        {
            //Generate notional coupon schedule.
            //Then generate payment schedule.
            IEnumerable<DateTime> cpnDates = notionalCouponSchedule;
            var dateSequenceFactory = ServiceLocator.GetImplementationOf<IDateSequenceFactory>();
            if ((notionalCouponSchedule == null) && specification.CouponFrequency != DateSequenceFrequency.NEVER)
            {
                //Has some coupons, but has not been passed in as a parameter...
                //All notional coupon dates
                cpnDates = dateSequenceFactory.CreateDateSequence(
                    new DateSpan(specification.FirstCouponDate, specification.RedemptionDate),
                    specification.CouponFrequency);
            }

            var cpnPaymentSchedule = couponPayments;
            if (cpnPaymentSchedule == null && cpnDates != null && cpnDates.Any())
            {
                //We have a coupon schedule... now generate payment schedule
                var couponPaymentDates = dateSequenceFactory.CreateBusinessDateSequence(cpnDates,
                                                               specification.HolidayCalendarRef,
                                                               specification.BusinessDayConvention);
                cpnPaymentSchedule = new CouponPaymentSchedule();
                foreach (var couponPaymentDate in couponPaymentDates)
                {
                    var cpnPaymentDate = couponPaymentDate.Date;
                    var recordDate = couponPaymentDate.AddDays(specification.RecordDateOffset).Date;
                    cpnPaymentSchedule.Add(cpnPaymentDate, new CouponPayment()
                                                               {
                                                                   PaymentDate = cpnPaymentDate,
                                                                   RecordDate = recordDate
                                                               });
                }
            }

            return new BondInstrument(specification, cpnDates, cpnPaymentSchedule);
        }

        public InterestRateSchedule FixedCoupon(DateTime firstCouponDate, Decimal fixedRate)
        {
            return new InterestRateSchedule {{firstCouponDate, fixedRate}};
        }
    }
}
