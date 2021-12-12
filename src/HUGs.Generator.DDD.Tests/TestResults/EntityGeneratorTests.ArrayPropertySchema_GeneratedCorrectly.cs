using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Entity
{
    public class SimpleArrayPropertyEntityId : EntityId<SimpleArrayPropertyEntity>
    {
        public SimpleArrayPropertyEntityId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class SimpleArrayPropertyEntity : HUGs.Generator.DDD.Framework.BaseModels.Entity<SimpleArrayPropertyEntityId>
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
