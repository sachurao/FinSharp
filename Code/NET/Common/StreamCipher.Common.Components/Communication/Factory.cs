using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;
using StreamCipher.Common.Interfaces.DataInterchange;
using StreamCipher.Common.Utilities.DataInterchange;

namespace StreamCipher.Common.Components.Communication
{
    static class Factory
    {
        static IDataStructureFormatter CreateFormatter(DataInterchangeFormat dataFormat = DataInterchangeFormat.TEXT_UTF8)
        {
            switch (dataFormat)
            {
                case DataInterchangeFormat.TEXT_UTF8:
                default:
                    return new Utf8Formatter();
            }
        }

        internal static T CreateCommunicationChannel<T>(
            CommunicationMode communicationType, 
            ICommunicationServiceConfig config, 
            Action<Exception> defaultExceptionHandler,
            int instanceNum) where T : ICommunicationChannel
        {
            switch (communicationType.ServiceBus)
            {
                case ServiceBusType.RABBITMQ:
                default:
                    return (T)Activator.CreateInstance(typeof(T), new object[] { instanceNum });
            }

        }
    }
}
