using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.DataInterchange
{
    public class Utf8Formatter : IDataInterchangeFormatter
    {
        private UTF8Encoding _encoding = new UTF8Encoding();

        #region IDataInterchangeFormatter

        public DataInterchangeFormat Format
        {
            get { return DataInterchangeFormat.TEXT_UTF8; }
        }

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
