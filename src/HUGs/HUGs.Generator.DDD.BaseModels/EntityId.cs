using System.Collections.Generic;

namespace HUGs.Generator.DDD.BaseModels
{
    public class EntityId<T> : ValueObject, IId<string>
    {
        public string Value { get; private set; }

        public EntityId(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new List<object>() { this.Value };
        }
    }
}