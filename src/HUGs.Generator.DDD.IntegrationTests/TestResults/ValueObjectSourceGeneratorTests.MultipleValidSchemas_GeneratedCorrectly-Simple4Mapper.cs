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
    public class Simple4Mapper : DbEntityMapper<Simple4, Simple4DbEntity>
    {
        public Simple4Mapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override Simple4DbEntity ToDbEntity(Simple4 obj)
        {
            return new Simple4DbEntity
            {
            	Text = obj.Text,
            	Number = obj.Number
            };
        }

        public override Simple4 ToDddObject(Simple4DbEntity obj)
        {
            return new Simple4
            (
            	obj.Text,
            	obj.Number
            );
        }
    }
}
