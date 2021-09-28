using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using My.Additional.Using1;
using My.Additional.Using2;

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
        public string Number { get; private set; }

        public SimpleAggregate(IId<SimpleAggregateId> id, string Number)
        {
            Id = id;
            this.Number = Number;
            OnInitialized();
        }

        private partial void OnInitialized();
    }
}
