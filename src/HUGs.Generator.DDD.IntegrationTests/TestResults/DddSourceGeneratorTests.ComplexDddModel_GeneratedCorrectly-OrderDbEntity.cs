using System;
using System.Linq;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.DbEntity
{
    public class OrderDbEntity
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<OrderItemDbEntity> Items { get; set; }

        public AddressDbEntity ShippingAddress { get; set; }

        public string State { get; set; }

    }
}
