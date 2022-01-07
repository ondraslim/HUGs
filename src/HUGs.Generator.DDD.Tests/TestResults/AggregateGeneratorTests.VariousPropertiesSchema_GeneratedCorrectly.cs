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
    public class ArrayPropertyAggregateId : EntityId<ArrayPropertyAggregate>
    {
        public ArrayPropertyAggregateId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class ArrayPropertyAggregate : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<ArrayPropertyAggregateId>
    {
        private List<OrderItem> _Items;

        public string Text { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;

        public double? Number { get; private set; }

        public ArrayPropertyAggregate(IId<ArrayPropertyAggregateId> id, string Text, IEnumerable<OrderItem> Items, double? Number)
        {
            Id = id;
            this.Text = Text;
            this._Items = Items.ToList();
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
