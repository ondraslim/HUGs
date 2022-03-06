using System;
using System.Linq;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.DbEntity
{
    public class OrderItemDbEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public uint Amount { get; set; }

    }
}
