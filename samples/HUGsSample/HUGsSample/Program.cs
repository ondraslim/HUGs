// See https://aka.ms/new-console-template for more information

using System.ComponentModel.Design;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Mappers;


var factory = new DbEntityMapperFactory(new ServiceContainer());

var orderItem = new OrderItem(new OrderItemId(Guid.NewGuid()), "my item", 100, 1);

var mapper = new OrderItemMapper(factory);

var entity = mapper.ToDbEntity(orderItem);
var dddObject = mapper.ToDddObject(entity);