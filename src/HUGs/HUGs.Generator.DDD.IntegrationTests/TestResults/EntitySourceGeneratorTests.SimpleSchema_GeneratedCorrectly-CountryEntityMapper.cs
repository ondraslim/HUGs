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
    public class OrderItemMapper : DbEntityMapper<OrderItemEntity, OrderItemDbEntity>
    {
        public OrderItemMapper(IDbEntityMapperFactory factory): base(factory)
        {
        }

        public OrderItemDbEntity ToDbEntity(OrderItemEntity obj)
        {
            return new OrderItemDbEntity
            {
            	Name = obj.Name,
            	Price = obj.Price,
            	Amount = obj.Amount
            };
        }

        public OrderItemDbEntity ToDbEntity(OrderItemEntity obj)
        {
            return new OrderItemDbEntity
            (
            	obj.Name,
            	obj.Price,
            	obj.Amount
            );
        }
    }
}
