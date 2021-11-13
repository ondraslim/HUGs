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
    public class SimpleValueObjectMapper : DbEntityMapper<SimpleValueObjectValueObject, SimpleValueObjectDbEntity>
    {
        public SimpleValueObjectMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public SimpleValueObjectDbEntity ToDbEntity(SimpleValueObjectValueObject obj)
        {
            return new SimpleValueObjectDbEntity{};
        }
    }
}
