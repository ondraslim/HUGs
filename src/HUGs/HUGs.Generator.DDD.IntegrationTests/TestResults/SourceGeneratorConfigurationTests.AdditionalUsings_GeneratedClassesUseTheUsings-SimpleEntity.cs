using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Entity
{
    public class SimpleEntityId : EntityId<SimpleEntity>
    {
        public SimpleEntityId(Guid value): base(value)
        {
        }
    }

    public partial class SimpleEntity : HUGs.Generator.DDD.Framework.BaseModels.Entity<SimpleEntityId>
    {
        public string Number { get; private set; }

        public SimpleEntity(IId<SimpleEntityId> id, string Number)
        {
            Id = id;
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
