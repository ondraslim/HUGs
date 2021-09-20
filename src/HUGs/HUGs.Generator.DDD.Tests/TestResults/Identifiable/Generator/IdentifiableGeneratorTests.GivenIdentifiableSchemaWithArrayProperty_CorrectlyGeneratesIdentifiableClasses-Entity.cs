using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class SimpleArrayPropertyEntityId : EntityId<SimpleArrayPropertyEntity>
    {
        public SimpleArrayPropertyEntityId(string value): base(value)
        {
        }
    }

    public partial class SimpleArrayPropertyEntity : HUGs.Generator.DDD.BaseModels.Entity<SimpleArrayPropertyEntityId>
    {
        private List<OrderItem> _Items;
        public IReadOnlyList<OrderItem> Items => _Items;
        public SimpleArrayPropertyEntity(IId<SimpleArrayPropertyEntityId> id, IEnumerable<OrderItem> Items)
        {
            Id = id;
            this._Items = Items.ToList();
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
