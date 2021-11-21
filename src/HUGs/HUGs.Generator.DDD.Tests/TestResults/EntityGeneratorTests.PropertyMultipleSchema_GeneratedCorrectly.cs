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
    public class PropertiesEntityId : EntityId<PropertiesEntity>
    {
        public PropertiesEntityId(Guid value): base(value)
        {
        }
    }

    public partial class PropertiesEntity : HUGs.Generator.DDD.Framework.BaseModels.Entity<PropertiesEntityId>
    {
        public string Text { get; private set; }

        public double? Number { get; private set; }

        public PropertiesEntity(IId<PropertiesEntityId> id, string Text, double? Number)
        {
            Id = id;
            this.Text = Text;
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
