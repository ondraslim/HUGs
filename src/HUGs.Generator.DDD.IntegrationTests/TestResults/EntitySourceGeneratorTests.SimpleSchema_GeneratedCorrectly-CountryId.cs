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
using HUGs.DDD.Generated.Mapper;

namespace HUGs.DDD.Generated.Entity
{
    public class CountryId : EntityId<Country>
    {
        public CountryId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class Country : HUGs.Generator.DDD.Framework.BaseModels.Entity<CountryId>
    {
        public string Name { get; private set; }

        public Country(IId<CountryId> id, string Name)
        {
            Id = id;
            this.Name = Name;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
