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
    public partial class Simple3Mapper : DbEntityMapper<Simple3, Simple3DbEntity>
    {
        public Simple3Mapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override Simple3DbEntity ToDbEntity(Simple3 obj)
        {
            return new Simple3DbEntity
            {
            	Text = obj.Text
            };
        }

        public override Simple3 ToDddObject(Simple3DbEntity obj)
        {
            return new Simple3
            (
            	obj.Text
            );
        }
    }
}
