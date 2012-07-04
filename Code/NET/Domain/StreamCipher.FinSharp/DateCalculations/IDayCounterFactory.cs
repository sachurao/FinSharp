namespace StreamCipher.FinSharp.DateCalculations
{
    public interface IDayCounterFactory
    {
        ICountDays Create(DayCountConvention dcc);
    }

    public enum DayCountConvention
    {
        DCC_ACT_ACT = 0,
        DCC_ACT_365 = 1,
        DCC_ACT_360 = 2,
        DCC_30_360 = 3,
        DCC_30E_360 = 4
    }
}
