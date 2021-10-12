using System;

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

        // TODO: implement
        //public abstract Enumeration FromString(string @string);

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

        // TODO: converter
    }



    public class OrderState : Enumeration
    {
        public static readonly OrderState Created = new(nameof(Created), "Created");
        public static readonly OrderState Canceled = new(nameof(Canceled), "Canceled");
        public string Name { get; }

        private OrderState(string internalName, string Name) : base(internalName)
        {
            this.Name = Name;
        }

        public static OrderState FromString(string @string)
        {
            return @string switch
            {
                nameof(Created) => Created,
                nameof(Canceled) => Canceled,
                _ => throw new ArgumentOutOfRangeException(nameof(@string), @string, null)
            };
        }
    }

}