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
    public class Simple4Mapper : DbEntityMapper<Simple4ValueObject, Simple4DbEntity>
    {
        public Simple4Mapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public Simple4DbEntity ToDbEntity(Simple4ValueObject obj)
        {
            return new Simple4DbEntity
            {
            	Text = obj.Text,
            	Number = obj.Number
            };
        }

        public Simple4DbEntity ToDbEntity(Simple4ValueObject obj)
        {
            return new Simple4DbEntity(obj.Text, obj.Number);
        }
    }
}
