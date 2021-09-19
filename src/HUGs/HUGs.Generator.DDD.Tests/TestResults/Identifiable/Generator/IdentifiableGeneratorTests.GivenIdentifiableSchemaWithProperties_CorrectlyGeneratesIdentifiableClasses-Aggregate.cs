using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class PropertiesAggregateId : EntityId<PropertiesAggregate>
    {
        public PropertiesAggregateId(string value)
        {
        }
    }

    public partial class PropertiesAggregate : Aggregate<PropertiesAggregateId>
    {
        public string Text { get; private set; }

        public double? Number { get; private set; }

        public PropertiesAggregate(PropertiesAggregateId id, string Text, double? Number): base(id)
        {
            this.Text = Text;
            this.Number = Number;
        }

        private partial void OnInitialized();
    }
}
