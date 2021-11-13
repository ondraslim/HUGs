namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public abstract class Enumeration
    {
        public string InternalName { get; }

        protected Enumeration(string internalName)
        {
            InternalName = internalName;
        }

        public override string ToString()
        {
            return InternalName;
        }
        public abstract Enumeration FromString(string @string);

        protected bool Equals(Enumeration other)
        {
            return InternalName == other.InternalName;
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
            return (InternalName != null ? InternalName.GetHashCode() : 0);
        }
    }

}