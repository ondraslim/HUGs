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

namespace HUGs.DDD.Generated.DbEntity
{
    public class Simple1Mapper : DbEntityMapper<Simple1ValueObject, Simple1DbEntity>
    {
        public Simple1Mapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public Simple1DbEntity ToDbEntity(Simple1ValueObject obj)
        {
            return new Simple1DbEntity
            {
            	Text = obj.Text
            };
        }

        public Simple1DbEntity ToDbEntity(Simple1ValueObject obj)
        {
            return new Simple1DbEntity
            (
            	obj.Text
            );
        }
    }
}
