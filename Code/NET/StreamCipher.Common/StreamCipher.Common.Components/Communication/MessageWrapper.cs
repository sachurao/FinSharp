using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;

namespace StreamCipher.Common.Components.Communication
{
    public class MessageWrapper:IMessageWrapper
    {
        private readonly string _correlationId;
        private readonly object _content;

        public MessageWrapper(object content) :this (content, new Guid().ToString())
        {
        }

        public MessageWrapper(object content, String correlationId)
        {
            _content = content;
            _correlationId = correlationId;
        }

        public string CorrelationId
        {
            get { return _correlationId; }
        }

        public object Content
        {
            get { return _content; }
        }
    }
}
