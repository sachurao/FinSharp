using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamCipher.Common.Interfaces.DataInterchange
{
    public interface IDataInterchangeFormatter
    {
        DataInterchangeFormat Format { get; }
        byte[] Serialize(Object o);
        Object Deserialize(byte[] rawData);
    }

    public enum DataInterchangeFormat
    {
        TEXT_UTF8 = 0,
        PROTOBUF = 1,
        JSON = 2,
        CUSTOM = 3
    }
}
