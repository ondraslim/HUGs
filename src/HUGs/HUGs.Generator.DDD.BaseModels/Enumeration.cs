using System;

namespace HUGs.Generator.DDD.BaseModels
{
    public abstract class Enumeration
    {
        private string _internalName;

        protected Enumeration(string internalName)
        {
            _internalName = internalName;
        }

        public override string ToString()
        {
            return _internalName;
        }

        // TODO: implement
        public abstract Enumeration FromString(string @string);

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