using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;
using HUGs.DDD.Generated.DbEntity;
using HUGs.DDD.Generated.Mapper;

namespace HUGs.DDD.Generated.Aggregate
{
    public class PropertiesAggregateId : EntityId<PropertiesAggregate>
    {
        public PropertiesAggregateId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class PropertiesAggregate : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<PropertiesAggregateId>
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
