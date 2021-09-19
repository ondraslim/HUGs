using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class SimpleAggregateId : EntityId<SimpleAggregate>
    {
        public SimpleAggregateId(string value)
        {
        }
    }

    public partial class SimpleAggregate : Aggregate<SimpleAggregateId>
    {
        public SimpleAggregate(SimpleAggregateId id): base(id)
        {
        }

        private partial void OnInitialized();
    }
}
