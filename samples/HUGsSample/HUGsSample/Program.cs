// See https://aka.ms/new-console-template for more information

using System.ComponentModel.Design;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Mappers;
using My.Desired.Namespace.ValueObjects;

var factory = new DbEntityMapperFactory(new ServiceContainer());

var orderItem = new OrderItem(new OrderItemId(Guid.NewGuid()), "my item", 100, 1);
var mapper = new OrderItemMapper(factory);
var entity = mapper.ToDbEntity(orderItem);
var dddObject = mapper.ToDddObject(entity);

var address = new Address("Main street", null, "Capital", "11111", new CountryId(Guid.NewGuid()));
var addressMapper = new AddressMapper(factory);
var addressEntity = addressMapper.ToDbEntity(address);
var addressDddObject = addressMapper.ToDddObject(addressEntity);

Console.ReadKey();