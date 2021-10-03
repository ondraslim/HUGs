using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Entity
{
    public class CountryId : EntityId<Country>
    {
        public CountryId(Guid value): base(value)
        {
        }
    }

    public partial class Country : HUGs.Generator.DDD.BaseModels.Entity<CountryId>
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
