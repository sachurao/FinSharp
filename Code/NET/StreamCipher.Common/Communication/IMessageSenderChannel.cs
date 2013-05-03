using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamCipher.Common.Communication
{
    public interface IMessageSenderChannel : ICommunicationChannel
    {
        void Send(IMessageDestination destination, IMessageWrapper message,
            IMessageDestination responseDestination = null);
    }
}
