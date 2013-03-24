using StreamCipher.Common.Interfaces.DataInterchange;

namespace StreamCipher.Common.Communication
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
            if (obj == null) return false;
            CommunicationMode other = obj as CommunicationMode;
            if (other == null) return false;
            return ((other.ServiceBus == this.ServiceBus) && (other.Format == this.Format));
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                // Suitable nullity checks etc, of course :)
                hash = hash * 31 + ServiceBus.GetHashCode();
                hash = hash * 31 + Format.GetHashCode();
                return hash;
            }
        }
    }
}
