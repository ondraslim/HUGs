namespace HUGs.Generator.DDD.BaseModels
{
    public class Enumeration
    {
        private string _internalName;

        public Enumeration(string internalName)
        {
            _internalName = internalName;
        }

        protected bool Equals(Enumeration other)
        {
            return _internalName == other._internalName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Enumeration) obj);
        }

        public override int GetHashCode()
        {
            return (_internalName != null ? _internalName.GetHashCode() : 0);
        }

        // TODO: converter
    }

}