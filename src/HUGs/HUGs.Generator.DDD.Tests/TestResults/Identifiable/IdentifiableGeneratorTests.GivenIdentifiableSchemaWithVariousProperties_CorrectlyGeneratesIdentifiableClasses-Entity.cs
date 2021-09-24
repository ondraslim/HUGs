using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class ArrayPropertyEntityId : EntityId<ArrayPropertyEntity>
    {
        public ArrayPropertyEntityId(string value): base(value)
        {
        }
    }

    public partial class ArrayPropertyEntity : HUGs.Generator.DDD.BaseModels.Entity<ArrayPropertyEntityId>
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

        private partial void OnInitialized();
    }
}
