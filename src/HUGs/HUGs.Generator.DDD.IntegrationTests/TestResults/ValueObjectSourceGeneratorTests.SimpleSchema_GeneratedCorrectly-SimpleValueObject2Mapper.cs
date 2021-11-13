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
    public class Simple2Mapper : DbEntityMapper<Simple2ValueObject, Simple2DbEntity>
    {
        public Simple2Mapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public Simple2DbEntity ToDbEntity(Simple2ValueObject obj)
        {
            return new Simple2DbEntity
            {
            	Text = obj.Text,
            	Number = obj.Number
            };
        }

        public Simple2DbEntity ToDbEntity(Simple2ValueObject obj)
        {
            return new Simple2DbEntity(obj.Text, obj.Number);
        }
    }
}
