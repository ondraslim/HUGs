using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class ArrayPropertyAggregateId : EntityId<ArrayPropertyAggregate>
    {
        public ArrayPropertyAggregateId(string value): base(value)
        {
        }
    }

    public partial class ArrayPropertyAggregate : HUGs.Generator.DDD.BaseModels.Aggregate<ArrayPropertyAggregateId>
    {
        private IReadOnlyList<OrderItem> _Items;
        public string Text { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;
        public double? Number { get; private set; }

        public ArrayPropertyAggregate(IId<ArrayPropertyAggregateId> id, string Text, IReadOnlyList<OrderItem> Items, double? Number)
        {
            Id = id;
            this.Text = Text;
            this._Items = Items;
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
