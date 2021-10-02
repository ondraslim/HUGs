using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class SimpleEntityId : EntityId<SimpleEntity>
    {
        public SimpleEntityId(Guid value): base(value)
        {
        }
    }

    public partial class SimpleEntity : HUGs.Generator.DDD.BaseModels.Entity<SimpleEntityId>
    {
        public SimpleEntity(IId<SimpleEntityId> id)
        {
            Id = id;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
