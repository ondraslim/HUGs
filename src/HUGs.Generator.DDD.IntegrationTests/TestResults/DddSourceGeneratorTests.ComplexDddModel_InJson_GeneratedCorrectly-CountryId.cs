using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Entities
{
    public class CountryId : EntityId<Country>
    {
        public CountryId(Guid value) 
        	: base(value)
        {
        }
    }

    public partial class Country : HUGs.Generator.DDD.Framework.BaseModels.Entity<Guid>
    {
        public string Name { get; private set; }

        public Country(CountryId id, string Name)
        {
            Id = id;
            this.Name = Name;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
