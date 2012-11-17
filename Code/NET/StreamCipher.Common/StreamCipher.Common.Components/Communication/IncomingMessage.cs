using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Interfaces.Communication;

namespace StreamCipher.Common.Components.Communication
{
    public class IncomingMessage:MessageWrapper, IIncomingMessage
    {
        #region Member Variables

        private readonly IMessageDestination _sender;
        private readonly IMessageDestination _topic;

        #endregion

        public IncomingMessage(object content, String correlationId, 
            IMessageDestination sender, IMessageDestination topic) : base(content, correlationId)
        {
            _sender = sender;
            _topic = topic;
        }

        #region Implementation of IIncomingMessage

        public IMessageDestination Sender
        {
            get { return _sender; }
        }

        public IMessageDestination Topic
        {
            get { return _topic; }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            IncomingMessage other = obj as IncomingMessage;
            if (other == null) return false;
            return ((other.Sender.Equals(this.Sender)) && (other.Topic == this.Topic));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                // Suitable nullity checks etc, of course :)
                hash = hash * 31 + Sender.GetHashCode();
                hash = hash * 31 + Topic.GetHashCode();
                return hash;
            }
        }
    }
}
