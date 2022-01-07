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

namespace HUGs.DDD.Generated.Aggregate
{
    public class SimpleAggregateId : EntityId<SimpleAggregate>
    {
        public SimpleAggregateId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class SimpleAggregate : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<SimpleAggregateId>
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
