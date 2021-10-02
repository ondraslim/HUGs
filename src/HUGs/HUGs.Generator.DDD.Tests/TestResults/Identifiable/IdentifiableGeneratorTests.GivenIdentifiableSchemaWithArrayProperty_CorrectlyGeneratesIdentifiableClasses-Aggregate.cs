using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class SimpleArrayPropertyAggregateId : EntityId<SimpleArrayPropertyAggregate>
    {
        public SimpleArrayPropertyAggregateId(Guid value): base(value)
        {
        }
    }

    public partial class SimpleArrayPropertyAggregate : HUGs.Generator.DDD.BaseModels.Aggregate<SimpleArrayPropertyAggregateId>
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
