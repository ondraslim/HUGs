using System;
using System.Linq;
using System.Collections.Generic;

namespace My.Desired.Namespace.DbEntities
{
    public class OrderItemDbEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public uint Amount { get; set; }

    }
}
