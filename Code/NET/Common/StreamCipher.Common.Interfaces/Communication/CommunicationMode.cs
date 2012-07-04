using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Interfaces.Communication
{
    public class CommunicationMode
    {
        public ServiceBusType ServiceBus { get; set; }
        public DataInterchangeFormat Format { get; set; }

        public CommunicationMode(ServiceBusType serviceBus, DataInterchangeFormat format)
        {
            ServiceBus = serviceBus;
            Format = format;
        }

        public override bool Equals(object obj)
        {
            CommunicationMode other = obj as CommunicationMode;
            if ((other == null) || 
                (other.ServiceBus != this.ServiceBus || other.Format != this.Format)) return false;
            else return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
