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
    public class Simple3Mapper : DbEntityMapper<Simple3ValueObject, Simple3DbEntity>
    {
        public Simple3Mapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public Simple3DbEntity ToDbEntity(Simple3ValueObject obj)
        {
            return new Simple3DbEntity
            {
            	Text = obj.Text
            };
        }

        public Simple3DbEntity ToDbEntity(Simple3ValueObject obj)
        {
            return new Simple3DbEntity(obj.Text);
        }
    }
}
