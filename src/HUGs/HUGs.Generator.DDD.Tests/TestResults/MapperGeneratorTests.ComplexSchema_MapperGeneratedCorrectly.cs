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

        public override SimpleAggregateDbEntity ToDbEntity(SimpleAggregateAggregate obj)
        {
            return new SimpleAggregateDbEntity
            {
            	SimpleString = obj.SimpleString,
            	SimpleNumber = obj.SimpleNumber,
            	SimpleOptional = obj.SimpleOptional,
            	SimpleCollection = MapDbEntityCollection(obj.SimpleCollection),
            	SimpleEntity = MapChildDbEntity(obj.SimpleEntity),
            	SimpleValueObject = MapChildDbEntity(obj.SimpleValueObject),
            	SimpleEntityId = MapDbEntityId(obj.SimpleEntityId)
            };
        }

        public override SimpleAggregateAggregate ToDddObject(SimpleAggregateDbEntity obj)
        {
            return new SimpleAggregateAggregate
            (
            	obj.SimpleString,
            	obj.SimpleNumber,
            	obj.SimpleOptional,
            	MapDddObjectCollection(obj.SimpleCollection),
            	MapChildDddObject(obj.SimpleEntity),
            	MapChildDddObject(obj.SimpleValueObject),
            	MapDddObjectId(obj.SimpleEntityId)
            );
        }
    }
}
