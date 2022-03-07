using System;
using System.Linq;
using System.Collections.Generic;

namespace My.Desired.Namespace.DbEntities
{
    public class AddressDbEntity
    {
        public string Street { get; set; }

        public string? Street2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public Guid CountryId { get; set; }

    }
}
