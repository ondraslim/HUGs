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
    public class SimpleOptionalMapper : DbEntityMapper<SimpleOptional, SimpleOptionalDbEntity>
    {
        public SimpleOptionalMapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override SimpleOptionalDbEntity ToDbEntity(SimpleOptional obj)
        {
            return new SimpleOptionalDbEntity
            {
            	Text = obj.Text,
            	Number = obj.Number,
            	TextOptional = obj.TextOptional,
            	NumberOptional = obj.NumberOptional
            };
        }

        public override SimpleOptional ToDddObject(SimpleOptionalDbEntity obj)
        {
            return new SimpleOptional
            (
            	obj.Text,
            	obj.Number,
            	obj.TextOptional,
            	obj.NumberOptional
            );
        }
    }
}
