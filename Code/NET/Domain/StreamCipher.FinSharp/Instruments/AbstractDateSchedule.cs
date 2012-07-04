using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.FinSharp.Instruments
{
    public abstract class AbstractDateSchedule<TValue>:Dictionary<DateTime, TValue>
    {
    }
}
