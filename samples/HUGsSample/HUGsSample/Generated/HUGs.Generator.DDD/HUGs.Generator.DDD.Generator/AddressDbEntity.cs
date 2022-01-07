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

namespace My.Desired.Namespace.DbEntities
{
    public class AddressDbEntity
    {
        public string Street { get; set; }

        public string? Street2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public CountryId CountryId { get; set; }

    }
}