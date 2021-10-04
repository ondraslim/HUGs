using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

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
