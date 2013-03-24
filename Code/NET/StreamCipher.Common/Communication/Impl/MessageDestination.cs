namespace StreamCipher.Common.Communication.Impl
{
    public class MessageDestination:IMessageDestination
    {
        private readonly string _address;

        public MessageDestination(string address)
        {
            _address = address;
        }
        
        public string Address
        {
            get { return _address; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            MessageDestination other = obj as MessageDestination;
            if (other == null) return false;
            return (other.Address == this.Address);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 23;
                // Suitable nullity checks etc, of course :)
                hash = hash * 31 + Address.GetHashCode();
                return hash;
            }
        }

        #region Overrides of Object

        public override string ToString()
        {
            return _address;
        }

        #endregion
    }
}
