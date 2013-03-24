using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.DataInterchange
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
