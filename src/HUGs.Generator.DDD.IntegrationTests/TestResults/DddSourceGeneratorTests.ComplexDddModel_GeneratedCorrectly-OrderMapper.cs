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

namespace HUGs.DDD.Generated.Mapper
{
    public class OrderMapper : DbEntityMapper<Order, OrderDbEntity>
    {
        public OrderMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override OrderDbEntity ToDbEntity(Order obj)
        {
            return new OrderDbEntity
            {
            	Id = ToDbEntityId(obj.Id),
            	Number = obj.Number,
            	CreatedDate = obj.CreatedDate,
            	Items = ToDbEntityCollection<OrderItem, OrderItemDbEntity>(obj.Items),
            	ShippingAddress = ToChildDbEntity<Address, AddressDbEntity>(obj.ShippingAddress),
            	State = ToDbEntityEnumeration(obj.State)
            };
        }

        public override Order ToDddObject(OrderDbEntity obj)
        {
            return new Order
            (
            	ToDddObjectId<OrderId>(obj.Id),
            	obj.Number,
            	obj.CreatedDate,
            	ToDddObjectCollection<OrderItemDbEntity, OrderItem>(obj.Items),
            	ToChildDddObject<AddressDbEntity, Address>(obj.ShippingAddress),
            	ToDddObjectEnumeration<OrderState>(obj.State)
            );
        }
    }
}
