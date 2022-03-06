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
    public class Simple2Mapper : DbEntityMapper<Simple2, Simple2DbEntity>
    {
        public Simple2Mapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override Simple2DbEntity ToDbEntity(Simple2 obj)
        {
            return new Simple2DbEntity
            {
            	Text = obj.Text,
            	Number = obj.Number
            };
        }

        public override Simple2 ToDddObject(Simple2DbEntity obj)
        {
            return new Simple2
            (
            	obj.Text,
            	obj.Number
            );
        }
    }
}
