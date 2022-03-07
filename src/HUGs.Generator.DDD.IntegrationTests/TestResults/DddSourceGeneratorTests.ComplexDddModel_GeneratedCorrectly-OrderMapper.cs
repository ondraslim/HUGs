using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;
using My.Desired.Namespace.DbEntities;

namespace My.Desired.Namespace.Mappers
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
