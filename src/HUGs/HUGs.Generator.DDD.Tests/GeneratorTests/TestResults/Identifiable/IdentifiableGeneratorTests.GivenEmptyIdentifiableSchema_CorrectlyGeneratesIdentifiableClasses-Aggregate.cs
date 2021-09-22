using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class SimpleAggregateId : EntityId<SimpleAggregate>
    {
        public SimpleAggregateId(string value): base(value)
        {
        }
    }

    public partial class SimpleAggregate : HUGs.Generator.DDD.BaseModels.Aggregate<SimpleAggregateId>
    {
        public SimpleAggregate(IId<SimpleAggregateId> id)
        {
            Id = id;
            OnInitialized();
        }

        private partial void OnInitialized();
    }
}
