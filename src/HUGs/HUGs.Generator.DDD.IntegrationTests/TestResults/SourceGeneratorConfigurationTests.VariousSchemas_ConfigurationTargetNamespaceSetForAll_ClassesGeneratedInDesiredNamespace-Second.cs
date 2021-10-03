using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Aggregates
{
    public class SimpleAggregateId : EntityId<SimpleAggregate>
    {
        public SimpleAggregateId(Guid value): base(value)
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

        partial void OnInitialized();
    }
}
