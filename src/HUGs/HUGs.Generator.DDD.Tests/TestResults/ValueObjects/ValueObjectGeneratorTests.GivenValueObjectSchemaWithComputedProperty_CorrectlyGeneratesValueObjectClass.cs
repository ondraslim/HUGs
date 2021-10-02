using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class SimpleOptionalPropertyClass : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public int Number { get; }

        public int ComputedNumber { get; }

        public SimpleOptionalPropertyClass(int Number)
        {
            this.Number = Number;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return ComputedNumber;
        }

        partial void OnInitialized();
    }
}
