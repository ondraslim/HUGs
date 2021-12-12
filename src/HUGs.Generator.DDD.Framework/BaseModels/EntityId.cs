using System;
using System.Collections.Generic;

namespace HUGs.Generator.DDD.Framework.BaseModels
{
    public class EntityId<T> : ValueObject, IId<Guid>
    {
        public Guid Value { get; private set; }

        public EntityId(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new List<object> { Value };
        }
    }
}