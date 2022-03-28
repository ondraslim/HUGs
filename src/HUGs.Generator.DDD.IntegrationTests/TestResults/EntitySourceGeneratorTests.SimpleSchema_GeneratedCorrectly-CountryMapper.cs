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
    public partial class CountryMapper : DbEntityMapper<Country, CountryDbEntity>
    {
        public CountryMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override CountryDbEntity ToDbEntity(Country obj)
        {
            return new CountryDbEntity
            {
            	Id = ToDbEntityId(obj.Id),
            	Name = obj.Name
            };
        }

        public override Country ToDddObject(CountryDbEntity obj)
        {
            return new Country
            (
            	ToDddObjectId<CountryId>(obj.Id),
            	obj.Name
            );
        }
    }
}
