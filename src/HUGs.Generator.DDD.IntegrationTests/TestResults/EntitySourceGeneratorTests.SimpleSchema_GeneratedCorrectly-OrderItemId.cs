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

namespace HUGs.DDD.Generated.Entity
{
    public class OrderItemId : EntityId<OrderItem>
    {
        public OrderItemId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class OrderItem : HUGs.Generator.DDD.Framework.BaseModels.Entity<OrderItemId>
    {
        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public uint Amount { get; private set; }

        public OrderItem(IId<OrderItemId> id, string Name, decimal Price, uint Amount)
        {
            Id = id;
            this.Name = Name;
            this.Price = Price;
            this.Amount = Amount;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
