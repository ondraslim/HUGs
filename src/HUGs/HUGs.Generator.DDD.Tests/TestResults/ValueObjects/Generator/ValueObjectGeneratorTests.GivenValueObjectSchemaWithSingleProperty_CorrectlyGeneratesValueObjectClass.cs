using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class SimpleOptionalPropertyClass : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public int Number { get; }

        public SimpleOptionalPropertyClass(int Number)
        {
            this.Number = Number;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
        }
    }
}
