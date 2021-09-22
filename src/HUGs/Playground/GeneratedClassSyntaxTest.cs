//using System;
//using System.Linq;
//using System.Collections.Generic;
//using HUGs.Generator.DDD.BaseModels;

//namespace HUGs.DDD.Generated.Aggregate
//{
//    public class OrderId : EntityId<Order>
//    {
//        public OrderId(string value) : base(value)
//        {
//        }
//    }

//    public partial class Order : HUGs.Generator.DDD.BaseModels.Aggregate<OrderId>
//    {
//        private List<OrderItem> _Items;
//        public string Number { get; private set; }

//        public DateTime CreatedDate { get; private set; }

//        public IReadOnlyList<OrderItem> Items => _Items;
//        public Address ShippingAddress { get; private set; }

//        public decimal TotalPrice { get; private set; }

//        public Order(IId<OrderId> id, string Number, DateTime CreatedDate, IEnumerable<OrderItem> Items, Address ShippingAddress)
//        {
//            Id = id;
//            this.Number = Number;
//            this.CreatedDate = CreatedDate;
//            this._Items = Items.ToList();
//            this.ShippingAddress = ShippingAddress;
//            OnInitialized();
//        }

//        private partial void OnInitialized();
//    }

//    public partial class Order
//    {
//        private partial void OnInitialized()
//        {


//        }
//    }
//}