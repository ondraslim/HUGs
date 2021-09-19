using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class SimpleEntityId : EntityId<SimpleEntity>
    {
        public SimpleEntityId(string value)
        {
        }
    }

    public partial class SimpleEntity : Entity<SimpleEntityId>
    {
        public SimpleEntity(SimpleEntityId id): base(id)
        {
        }

        private partial void OnInitialized();
    }
}
