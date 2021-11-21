using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.DbEntity
{
    public class OrderMapper : DbEntityMapper<Order, OrderDbEntity>
    {
        public OrderMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public override OrderDbEntity ToDbEntity(Order obj)
        {
            return new OrderDbEntity
            {
            	Number = obj.Number,
            	CreatedDate = obj.CreatedDate,
            	Items = MapDbEntityCollection(obj.Items),
            	ShippingAddress = MapChildDbEntity(obj.ShippingAddress),
            	State = MapDbEntityEnumeration(obj.State)
            };
        }

        public override Order ToDddObject(OrderDbEntity obj)
        {
            return new Order
            (
            	obj.Number,
            	obj.CreatedDate,
            	MapDddObjectCollection(obj.Items),
            	MapChildDddObject(obj.ShippingAddress),
            	MapDddObjectEnumeration(obj.State)
            );
        }
    }
}
