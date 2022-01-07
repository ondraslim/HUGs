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

namespace My.Desired.Namespace.ValueObjects
{
    public partial class Address : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
    {
        public string Street { get; }

        public string? Street2 { get; }

        public string City { get; }

        public string Zip { get; }

        public CountryId CountryId { get; }

        public Address(string Street, string? Street2, string City, string Zip, CountryId CountryId)
        {
            this.Street = Street;
            this.Street2 = Street2;
            this.City = City;
            this.Zip = Zip;
            this.CountryId = CountryId;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return Street2;
            yield return City;
            yield return Zip;
            yield return CountryId;
        }

        partial void OnInitialized();
    }
}