using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Aggregates
{
    public class OrderId : EntityId<Order>
    {
        public OrderId(Guid value) 
        	: base(value)
        {
        }
    }

    public partial class Order : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<Guid>
    {
        private List<OrderItem> _Items;

        public string Number { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;

        public Address ShippingAddress { get; private set; }

        public decimal TotalPrice { get; private set; }

        public OrderState State { get; private set; }

        public Order(OrderId id, string Number, DateTime CreatedDate, IEnumerable<OrderItem> Items, Address ShippingAddress, OrderState State)
        {
            Id = id;
            this.Number = Number;
            this.CreatedDate = CreatedDate;
            this._Items = Items.ToList();
            this.ShippingAddress = ShippingAddress;
            this.State = State;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
