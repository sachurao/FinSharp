using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.FinSharp.Instruments
{
    /// <summary>
    /// Can represent a cashflow schedule or a rate schedule.
    /// </summary>
    public abstract class AbstractDateDecimalSchedule:AbstractDateSchedule<Decimal>
    {
    }
}
