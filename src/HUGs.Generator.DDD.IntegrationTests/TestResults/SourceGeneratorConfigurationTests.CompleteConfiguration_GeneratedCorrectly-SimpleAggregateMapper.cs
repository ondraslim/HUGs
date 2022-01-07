using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;
using My.Desired.Namespace.DbEntities;
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
            	Number = obj.Number
            };
        }

        public override SimpleAggregate ToDddObject(SimpleAggregateDbEntity obj)
        {
            return new SimpleAggregate
            (
            	obj.Number
            );
        }
    }
}
