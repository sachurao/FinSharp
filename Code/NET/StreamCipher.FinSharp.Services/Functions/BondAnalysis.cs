using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamCipher.Common.Async;
using StreamCipher.Common.Interfaces.ActivityControl;
using StreamCipher.Common.Utilities;

namespace StreamCipher.FinSharp.Services.Functions
{
    class BondAnalysis : ControlledComponent
    {
        public BondAnalysis(): base("Bond Analysis")
        {
        }

        public int GetAccruedDays(string identifier)
        {
            //TODO:  Implement GetAccruedDays.
            return 10;
        }
    }

    public enum BondIdentifierType
    {
        UNKNOWN=0,
        ISIN = 1,
        CUSIP = 2
    }
}
