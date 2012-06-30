using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Communication;
using Irrefutable.Shared.Interfaces.DataInterchange;
using Irrefutable.Shared.Utilities.DataInterchange;

namespace Irrefutable.Shared.Components.Communication
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
