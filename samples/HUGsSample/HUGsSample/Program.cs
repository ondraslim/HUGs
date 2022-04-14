// See https://aka.ms/new-console-template for more information

using System;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Mappers;
using My.Desired.Namespace.ValueObjects;
using System.ComponentModel.Design;
using My.Desired.Namespace.Entities;


var factory = new DbEntityMapperFactory(new ServiceContainer());


var orderItem = new OrderItem(new OrderItemId(Guid.NewGuid()), "my item", 100, 1);
Console.WriteLine($"created order item: {orderItem}");

var mapper = new OrderItemMapper(factory);
var entity = mapper.ToDbEntity(orderItem);
Console.WriteLine($"order item mapped to db entity: {entity}");

var dddObject = mapper.ToDddObject(entity); 
Console.WriteLine($"order item entity mapped back to DDD object: {dddObject}");


var address = new Address("Main street", null, "Capital", "11111", new CountryId(Guid.NewGuid()));
Console.WriteLine($"created address: {address}");

var addressMapper = new AddressMapper(factory);
var addressEntity = addressMapper.ToDbEntity(address);
Console.WriteLine($"address mapped to db entity: {addressEntity}");

var addressDddObject = addressMapper.ToDddObject(addressEntity);
Console.WriteLine($"address entity mapped back to DDD object: {addressDddObject}");

Console.ReadKey();