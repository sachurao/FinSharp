using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irrefutable.FinSharp.Instruments
{
    public abstract class AbstractDateSchedule<TValue>:Dictionary<DateTime, TValue>
    {
    }
}
