using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;
using My.Desired.Namespace.DbEntities;

namespace My.Desired.Namespace.Mappers
{
    public class Simple1Mapper : DbEntityMapper<Simple1, Simple1DbEntity>
    {
        public Simple1Mapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override Simple1DbEntity ToDbEntity(Simple1 obj)
        {
            return new Simple1DbEntity
            {
            	Text = obj.Text
            };
        }

        public override Simple1 ToDddObject(Simple1DbEntity obj)
        {
            return new Simple1
            (
            	obj.Text
            );
        }
    }
}
