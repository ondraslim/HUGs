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
    public class OrderMapper : DbEntityMapper<OrderAggregate, OrderDbEntity>
    {
        public OrderMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public OrderDbEntity ToDbEntity(OrderAggregate obj)
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

        public OrderDbEntity ToDbEntity(OrderAggregate obj)
        {
            return new OrderDbEntity
            (
            	obj.Number,
            	obj.CreatedDate,
            	obj.Items,
            	obj.ShippingAddress,
            	obj.State
            );
        }
    }
}
