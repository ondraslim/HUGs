using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;
using HUGs.DDD.Generated.DbEntity;
using HUGs.DDD.Generated.Mapper;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class SimpleOptionalPropertyClass : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
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
