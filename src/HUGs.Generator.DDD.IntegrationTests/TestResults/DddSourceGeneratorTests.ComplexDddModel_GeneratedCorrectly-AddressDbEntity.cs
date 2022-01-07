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

namespace HUGs.DDD.Generated.DbEntity
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
