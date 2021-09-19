using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class ArrayPropertyEntityId : EntityId<ArrayPropertyEntity>
    {
        public ArrayPropertyEntityId(string value)
        {
        }
    }

    public partial class ArrayPropertyEntity : Entity<ArrayPropertyEntityId>
    {
        private List<OrderItem> _Items;
        public string Text { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;
        public double? Number { get; private set; }

        public ArrayPropertyEntity(ArrayPropertyEntityId id, string Text, IReadOnlyList<OrderItem> Items, double? Number): base(id)
        {
            this.Text = Text;
            this._Items = Items;
            this.Number = Number;
        }

        private partial void OnInitialized();
    }
}
