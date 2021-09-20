using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Aggregate
{
    public class PropertiesAggregateId : EntityId<PropertiesAggregate>
    {
        public PropertiesAggregateId(string value): base(value)
        {
        }
    }

    public partial class PropertiesAggregate : HUGs.Generator.DDD.BaseModels.Aggregate<PropertiesAggregateId>
    {
        public string Text { get; private set; }

        public double? Number { get; private set; }

        public PropertiesAggregate(IId<PropertiesAggregateId> id, string Text, double? Number)
        {
            Id = id;
            this.Text = Text;
            this.Number = Number;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
