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
using My.Desired.Namespace.Mappers;

namespace My.Desired.Namespace.DbEntities
{
    public class OrderDbEntity
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        public Address ShippingAddress { get; set; }

        public string State { get; set; }

    }
}