//using System;
//using System.Linq;
//using System.Collections.Generic;
//using HUGs.Generator.DDD.Framework.BaseModels;
//using HUGs.Generator.DDD.Framework.Mapping;
//using My.Desired.Namespace.Entities;
//using My.Desired.Namespace.Aggregates;
//using My.Desired.Namespace.ValueObjects;
//using My.Desired.Namespace.Enumerations;
//using My.Desired.Namespace.DbEntities;

//namespace My.Desired.Namespace.Mappers
//{
//    public class OrderItemMapper : DbEntityMapper<OrderItem, OrderItemDbEntity>
//    {
//        public OrderItemMapper(IDbEntityMapperFactory factory)
//            : base(factory)
//        {
//        }

//        public override OrderItemDbEntity ToDbEntity(OrderItem obj)
//        {
//            return new OrderItemDbEntity
//            {
//                Id = ToDbEntityId(obj.Id),
//                Name = obj.Name,
//                Price = obj.Price,
//                Amount = obj.Amount
//            };
//        }

//        public override OrderItem ToDddObject(OrderItemDbEntity obj)
//        {
//            return new OrderItem
//            (
//                ToDddObjectId<OrderItemId>(obj.Id),
//                obj.Name,
//                obj.Price,
//                obj.Amount
//            );
//        }
//    }
//}