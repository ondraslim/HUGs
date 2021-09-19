using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class PropertiesEntityId : EntityId<PropertiesEntity>
    {
        public PropertiesEntityId(string value): base(value)
        {
        }
    }

    public partial class PropertiesEntity : HUGs.Generator.DDD.BaseModels.Entity<PropertiesEntityId>
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
