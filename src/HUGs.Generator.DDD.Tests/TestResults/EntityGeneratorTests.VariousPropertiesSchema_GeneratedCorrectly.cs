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

namespace HUGs.DDD.Generated.Entity
{
    public class ArrayPropertyEntityId : EntityId<ArrayPropertyEntity>
    {
        public ArrayPropertyEntityId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class ArrayPropertyEntity : HUGs.Generator.DDD.Framework.BaseModels.Entity<ArrayPropertyEntityId>
    {
        private List<OrderItem> _Items;

        public string Text { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;

        public double? Number { get; private set; }

        public ArrayPropertyEntity(IId<ArrayPropertyEntityId> id, string Text, IEnumerable<OrderItem> Items, double? Number)
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
