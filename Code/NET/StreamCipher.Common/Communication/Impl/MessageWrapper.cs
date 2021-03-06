using System;

namespace StreamCipher.Common.Communication.Impl
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

        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            MessageWrapper other = obj as MessageWrapper;
            if (other == null) return false;
            return ((other.Content.Equals(this.Content)) && (other.CorrelationId == this.CorrelationId));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                // Suitable nullity checks etc, of course :)
                if (Content!=null) hash = hash * 31 + Content.GetHashCode();
                if (CorrelationId!=null) hash = hash * 31 + CorrelationId.GetHashCode();
                return hash;
            }
        }
    }
}
