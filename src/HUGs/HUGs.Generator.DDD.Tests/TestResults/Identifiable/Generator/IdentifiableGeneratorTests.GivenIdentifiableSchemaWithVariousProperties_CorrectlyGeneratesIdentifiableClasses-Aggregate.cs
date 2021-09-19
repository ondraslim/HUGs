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
        public string Text { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;
        public double? Number { get; private set; }

        public ArrayPropertyAggregate(ArrayPropertyAggregateId id, string Text, IReadOnlyList<OrderItem> Items, double? Number): base(id)
        {
            this.Text = Text;
            this._Items = Items;
            this.Number = Number;
        }

        private partial void OnInitialized();
    }
}
