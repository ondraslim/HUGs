using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.DbEntity
{
    public class SimpleAggregateMapper : DbEntityMapper<SimpleAggregateAggregate, SimpleAggregateDbEntity>
    {
        public SimpleAggregateMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public SimpleAggregateDbEntity ToDbEntity(SimpleAggregateAggregate obj)
        {
            return new SimpleAggregateDbEntity
            {
            	Number = obj.Number
            };
        }

        public SimpleAggregateDbEntity ToDbEntity(SimpleAggregateAggregate obj)
        {
            return new SimpleAggregateDbEntity
            (
            	obj.Number
            );
        }
    }
}
