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
    public class CountryMapper : DbEntityMapper<CountryEntity, CountryDbEntity>
    {
        public CountryMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public CountryDbEntity ToDbEntity(CountryEntity obj)
        {
            return new CountryDbEntity
            {
            	Name = obj.Name
            };
        }

        public CountryDbEntity ToDbEntity(CountryEntity obj)
        {
            return new CountryDbEntity
            (
            	obj.Name
            );
        }
    }
}
