using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class SimpleArrayPropertyAggregateId : EntityId<SimpleArrayPropertyAggregate>
    {
        public SimpleArrayPropertyAggregateId(string value): base(value)
        {
        }
    }

    public partial class SimpleArrayPropertyAggregate : HUGs.Generator.DDD.BaseModels.Aggregate<SimpleArrayPropertyAggregateId>
    {
        private IReadOnlyList<OrderItem> _Items;
        public IReadOnlyList<OrderItem> Items => _Items;
        public SimpleArrayPropertyAggregate(IId<SimpleArrayPropertyAggregateId> id, IReadOnlyList<OrderItem> Items)
        {
            Id = id;
            this._Items = Items;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
