using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Utilities.DataInterchange;

namespace StreamCipher.Common.ThirdParty.DataInterchange
{
    public class FormatterFactory
    {
        public IDataInterchangeFormatter CreateFormatter(DataInterchangeFormat format)
        {
            IDataInterchangeFormatter retVal;
            switch (format)
            {
                default:
                    retVal = new Utf8Formatter();
                    break;
            }
            return retVal;
        }
    }
}
