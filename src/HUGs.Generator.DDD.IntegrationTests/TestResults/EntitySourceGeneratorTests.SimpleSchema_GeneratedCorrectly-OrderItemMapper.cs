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
    public partial class OrderItemMapper : DbEntityMapper<OrderItem, OrderItemDbEntity>
    {
        public OrderItemMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override OrderItemDbEntity ToDbEntity(OrderItem obj)
        {
            return new OrderItemDbEntity
            {
            	Id = ToDbEntityId(obj.Id),
            	Name = obj.Name,
            	Price = obj.Price,
            	Amount = obj.Amount
            };
        }

        public override OrderItem ToDddObject(OrderItemDbEntity obj)
        {
            return new OrderItem
            (
            	ToDddObjectId<OrderItemId>(obj.Id),
            	obj.Name,
            	obj.Price,
            	obj.Amount
            );
        }
    }
}
