using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Mapper
{
    public class SimpleValueObjectMapper : DbEntityMapper<SimpleValueObject, SimpleValueObjectDbEntity>
    {
        public SimpleValueObjectMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override SimpleValueObjectDbEntity ToDbEntity(SimpleValueObject obj)
        {
            return new SimpleValueObjectDbEntity
            {
            };
        }

        public override SimpleValueObject ToDddObject(SimpleValueObjectDbEntity obj)
        {
            return new SimpleValueObject
            (
            );
        }
    }
}
