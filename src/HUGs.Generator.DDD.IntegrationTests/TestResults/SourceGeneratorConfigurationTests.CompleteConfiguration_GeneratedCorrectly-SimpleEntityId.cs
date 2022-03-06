using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Entities
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
        public string Number { get; private set; }

        public SimpleEntity(SimpleEntityId id, string Number)
        {
            Id = id;
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
