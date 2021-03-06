using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;
using HUGs.DDD.Generated.DbEntity;

namespace HUGs.DDD.Generated.Mapper
{
    public partial class SimpleEntityMapper : DbEntityMapper<SimpleEntity, SimpleEntityDbEntity>
    {
        public SimpleEntityMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override SimpleEntityDbEntity ToDbEntity(SimpleEntity obj)
        {
            return new SimpleEntityDbEntity
            {
            	Id = ToDbEntityId(obj.Id),
            	Number = obj.Number
            };
        }

        public override SimpleEntity ToDddObject(SimpleEntityDbEntity obj)
        {
            return new SimpleEntity
            (
            	ToDddObjectId<SimpleEntityId>(obj.Id),
            	obj.Number
            );
        }
    }
}
