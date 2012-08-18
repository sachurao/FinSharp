using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;

namespace StreamCipher.Common.Components.Communication
{
    public class MessageDestination:IMessageDestination
    {
        private readonly string _address;

        public MessageDestination(string address)
        {
            if (String.IsNullOrEmpty(address)) throw new NotImplementedException("address");
            _address = address;
        }
        
        public string Address
        {
            get { return _address; }
        }
    }
}
