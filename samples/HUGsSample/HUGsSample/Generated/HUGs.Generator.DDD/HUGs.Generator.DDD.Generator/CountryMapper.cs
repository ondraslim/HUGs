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
using My.Desired.Namespace.Mappers;

namespace My.Desired.Namespace.Mappers
{
    public class CountryMapper : DbEntityMapper<Country, CountryDbEntity>
    {
        public CountryMapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override CountryDbEntity ToDbEntity(Country obj)
        {
            return new CountryDbEntity
            {
            	Name = obj.Name
            };
        }

        public override Country ToDddObject(CountryDbEntity obj)
        {
            return new Country
            (
            	obj.Name
            );
        }
    }
}