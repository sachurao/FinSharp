using StreamCipher.FinSharp.Analytics.Model.Dates;

namespace StreamCipher.FinSharp.Analytics.TimeValue.DateCalculations
{
    public static class DayCounterFactory
    {
        public static ICountDays Create(DayCountConvention dcc)
        {
            ICountDays retVal = null;
            switch (dcc)
            {
                    case DayCountConvention.DCC_ACT_ACT:
                        retVal = new DayCounter_Act_Act();
                        break;
                    case DayCountConvention.DCC_ACT_365:
                        retVal = new DayCounter_Act_365();
                        break;
                    case DayCountConvention.DCC_ACT_360:
                        retVal = new DayCounter_Act_360();
                        break;
                    case DayCountConvention.DCC_30_360:
                        retVal = new DayCounter_30_360();
                        break;
                    case DayCountConvention.DCC_30E_360:
                        retVal = new DayCounter_30E_360();
                        break;
            }
            return retVal;
        }
    }
}
