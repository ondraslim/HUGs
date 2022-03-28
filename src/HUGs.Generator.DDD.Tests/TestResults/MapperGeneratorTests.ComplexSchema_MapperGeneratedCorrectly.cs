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

namespace HUGs.DDD.Generated.Mapper
{
    public partial class SimpleAggregateMapper : DbEntityMapper<SimpleAggregate, SimpleAggregateDbEntity>
    {
        public SimpleAggregateMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override SimpleAggregateDbEntity ToDbEntity(SimpleAggregate obj)
        {
            return new SimpleAggregateDbEntity
            {
            	Id = ToDbEntityId(obj.Id),
            	SimpleString = obj.SimpleString,
            	SimpleNumber = obj.SimpleNumber,
            	SimpleOptional = obj.SimpleOptional,
            	SimpleCollection = ToDbEntityCollection<int, int>(obj.SimpleCollection),
            	SimpleEntity = ToChildDbEntity<DddEntity, DddEntityDbEntity>(obj.SimpleEntity),
            	SimpleValueObject = ToChildDbEntity<DddValueObject, DddValueObjectDbEntity>(obj.SimpleValueObject),
            	SimpleEntityId = ToDbEntityId(obj.SimpleEntityId)
            };
        }

        public override SimpleAggregate ToDddObject(SimpleAggregateDbEntity obj)
        {
            return new SimpleAggregate
            (
            	ToDddObjectId<SimpleAggregateId>(obj.Id),
            	obj.SimpleString,
            	obj.SimpleNumber,
            	obj.SimpleOptional,
            	ToDddObjectCollection<int, int>(obj.SimpleCollection),
            	ToChildDddObject<DddEntityDbEntity, DddEntity>(obj.SimpleEntity),
            	ToChildDddObject<DddValueObjectDbEntity, DddValueObject>(obj.SimpleValueObject),
            	ToDddObjectId<DddEntityId>(obj.SimpleEntityId)
            );
        }
    }
}
