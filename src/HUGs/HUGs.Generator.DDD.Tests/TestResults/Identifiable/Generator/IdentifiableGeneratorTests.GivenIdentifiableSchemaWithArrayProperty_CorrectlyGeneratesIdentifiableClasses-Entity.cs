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
        public IReadOnlyList<OrderItem> Items => _Items;
        public ArrayPropertyEntity(ArrayPropertyEntityId id, IReadOnlyList<OrderItem> Items): base(id)
        {
            this._Items = Items;
        }

        private partial void OnInitialized();
    }
}
