using System;

namespace StreamCipher.Common.Communication.Impl
{
    public class IncomingMessage:MessageWrapper, IIncomingMessage
    {
        #region Member Variables

        private readonly String _sender;
        private readonly IMessageDestination _topic;
        private readonly IMessageDestination _replyTo;
        private readonly DateTime _timeOfArrival;

        #endregion

        public IncomingMessage(object content, String correlationId, 
            String sender, IMessageDestination topic,
            IMessageDestination replyTo) : base(content, correlationId)
        {
            _sender = sender;
            _topic = topic;
            _replyTo = replyTo;
            _timeOfArrival = DateTime.UtcNow;
        }

        #region Implementation of IIncomingMessage

        public string Sender
        {
            get { return _sender; }
        }

        public IMessageDestination Topic
        {
            get { return _topic; }
        }

        public IMessageDestination ReplyTo
        {
            get { return _replyTo; }
        }

        public DateTime TimeOfArrival
        {
            get { return _timeOfArrival; }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            IncomingMessage other = obj as IncomingMessage;
            if (other == null) return false;
            return ((other.Sender.Equals(this.Sender)) && (other.Topic == this.Topic)
                && (other.CorrelationId == this.CorrelationId)
                && (other.Content.Equals(this.Content)));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                // Suitable nullity checks etc, of course :)
                if (Sender!=null)
                hash = hash * 31 + Sender.GetHashCode();
                if (Topic!=null)
                hash = hash * 31 + Topic.GetHashCode();
                if (CorrelationId!=null)
                hash = hash*31 + CorrelationId.GetHashCode();
                if (Content!=null)
                hash = hash*31 + Content.GetHashCode();
                return hash;
            }
        }
    }
}
