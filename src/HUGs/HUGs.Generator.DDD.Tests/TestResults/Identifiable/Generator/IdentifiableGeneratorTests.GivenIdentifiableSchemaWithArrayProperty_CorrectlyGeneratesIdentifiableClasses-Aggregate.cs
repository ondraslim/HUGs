using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class ArrayPropertyAggregateId : EntityId<ArrayPropertyAggregate>
    {
        public ArrayPropertyAggregateId(string value)
        {
        }
    }

    public partial class ArrayPropertyAggregate : Aggregate<ArrayPropertyAggregateId>
    {
        private List<OrderItem> _Items;
        public IReadOnlyList<OrderItem> Items => _Items;
        public ArrayPropertyAggregate(ArrayPropertyAggregateId id, IReadOnlyList<OrderItem> Items): base(id)
        {
            this._Items = Items;
        }

        private partial void OnInitialized();
    }
}
