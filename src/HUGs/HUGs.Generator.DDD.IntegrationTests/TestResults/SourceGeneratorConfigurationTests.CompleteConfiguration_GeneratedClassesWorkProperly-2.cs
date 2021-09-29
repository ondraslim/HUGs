using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using My.Additional.Using1;
using My.Additional.Using2;

namespace My.Desired.Namespace.Entities
{
    public class SimpleEntityId : EntityId<SimpleEntity>
    {
        public SimpleEntityId(string value): base(value)
        {
        }
    }

    public partial class SimpleEntity : HUGs.Generator.DDD.BaseModels.Entity<SimpleEntityId>
    {
        public string Number { get; private set; }

        public SimpleEntity(IId<SimpleEntityId> id, string Number)
        {
            Id = id;
            this.Number = Number;
            OnInitialized();
        }

        private partial void OnInitialized();
    }
}
