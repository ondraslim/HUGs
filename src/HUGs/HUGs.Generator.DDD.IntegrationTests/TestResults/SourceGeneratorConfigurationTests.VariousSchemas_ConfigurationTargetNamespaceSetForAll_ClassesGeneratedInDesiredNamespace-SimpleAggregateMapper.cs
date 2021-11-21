using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace HUGs.DDD.Generated.DbEntity
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
