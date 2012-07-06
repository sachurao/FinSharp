using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Utilities.DataInterchange
{
    public class Utf8Formatter : IDataStructureFormatter
    {
        private UTF8Encoding _encoding = new UTF8Encoding();

        #region IDataStructureFormatter

        public byte[] Serialize(object o)
        {
            return _encoding.GetBytes(o.ToString());
        }

        public object Deserialize(byte[] rawData)
        {
            return _encoding.GetString(rawData);
        }

        #endregion
    }
}
