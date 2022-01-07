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

namespace HUGs.DDD.Generated.Mapper
{
    public class SimpleAggregateMapper : DbEntityMapper<SimpleAggregate, SimpleAggregateDbEntity>
    {
        public SimpleAggregateMapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override SimpleAggregateDbEntity ToDbEntity(SimpleAggregate obj)
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

        public override SimpleAggregate ToDddObject(SimpleAggregateDbEntity obj)
        {
            return new SimpleAggregate
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
