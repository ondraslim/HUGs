
using System;
using System.ComponentModel.Design;
using My.Desired.Namespace.Entities;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Mappers;
using My.Desired.Namespace.ValueObjects;

namespace Playground
{
    internal class Program
    {
        private static void Main()
        {
            var factory = new DbEntityMapperFactory(new ServiceContainer());

            var orderItem = new OrderItem(new OrderItemId(Guid.NewGuid()), "my item", 100, 1);
            Console.WriteLine(orderItem);
            var mapper = new OrderItemMapper(factory);

            var entity = mapper.ToDbEntity(orderItem);
            Console.WriteLine(entity);
            var dddObject = mapper.ToDddObject(entity);
            Console.WriteLine(dddObject);

        }
    }
}

