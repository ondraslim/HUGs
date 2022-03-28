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
    public partial class AddressMapper : DbEntityMapper<Address, AddressDbEntity>
    {
        public AddressMapper(IDbEntityMapperFactory factory) 
        	: base(factory)
        {
        }

        public override AddressDbEntity ToDbEntity(Address obj)
        {
            return new AddressDbEntity
            {
            	Street = obj.Street,
            	Street2 = obj.Street2,
            	City = obj.City,
            	Zip = obj.Zip,
            	CountryId = ToDbEntityId(obj.CountryId)
            };
        }

        public override Address ToDddObject(AddressDbEntity obj)
        {
            return new Address
            (
            	obj.Street,
            	obj.Street2,
            	obj.City,
            	obj.Zip,
            	ToDddObjectId<CountryId>(obj.CountryId)
            );
        }
    }
}
