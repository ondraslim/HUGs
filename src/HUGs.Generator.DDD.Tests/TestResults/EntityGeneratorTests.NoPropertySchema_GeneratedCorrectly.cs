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
    public class SimpleEntityId : EntityId<SimpleEntity>
    {
        public SimpleEntityId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class SimpleEntity : HUGs.Generator.DDD.Framework.BaseModels.Entity<Guid>
    {
        public SimpleEntity(IId<SimpleEntityId> id)
        {
            Id = id;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
