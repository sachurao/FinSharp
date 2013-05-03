using System;
using System.Collections.Generic;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Communication
{
    
    public interface ICommunicationServiceConfig
    {
        String ServiceBusAddress { get; }
        String ConnectionIdPrefix { get; }
        String UniqueResponseTopic { get; }
        int TotalSenderChannels { get; }
        int TotalReceiverChannels { get; }
        IDictionary<String, String> CustomProps { get; }
        IDataInterchangeFormatter Formatter { get; }
        Action<Exception> DefaultExceptionHandler { get; }

    }
}
