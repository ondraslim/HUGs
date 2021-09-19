using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Entity
{
    public class PropertiesEntityId : EntityId<PropertiesEntity>
    {
        public PropertiesEntityId(string value)
        {
        }
    }

    public partial class PropertiesEntity : Entity<PropertiesEntityId>
    {
        public string Text { get; private set; }

        public double? Number { get; private set; }

        public PropertiesEntity(PropertiesEntityId id, string Text, double? Number): base(id)
        {
            this.Text = Text;
            this.Number = Number;
        }

        private partial void OnInitialized();
    }
}
