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
    public class SimpleArrayPropertyAggregateId : EntityId<SimpleArrayPropertyAggregate>
    {
        public SimpleArrayPropertyAggregateId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class SimpleArrayPropertyAggregate : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<SimpleArrayPropertyAggregateId>
    {
        private List<OrderItem> _Items;

        public IReadOnlyList<OrderItem> Items => _Items;

        public SimpleArrayPropertyAggregate(IId<SimpleArrayPropertyAggregateId> id, IEnumerable<OrderItem> Items)
        {
            Id = id;
            this._Items = Items.ToList();
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
