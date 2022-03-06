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
    public class SimpleEntityMapper : DbEntityMapper<SimpleEntity, SimpleEntityDbEntity>
    {
        public SimpleEntityMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override SimpleEntityDbEntity ToDbEntity(SimpleEntity obj)
        {
            return new SimpleEntityDbEntity
            {
            	Id = ToDbEntityId(obj.Id)
            };
        }

        public override SimpleEntity ToDddObject(SimpleEntityDbEntity obj)
        {
            return new SimpleEntity
            (
            	ToDddObjectId<SimpleEntityId>(obj.Id)
            );
        }
    }
}
