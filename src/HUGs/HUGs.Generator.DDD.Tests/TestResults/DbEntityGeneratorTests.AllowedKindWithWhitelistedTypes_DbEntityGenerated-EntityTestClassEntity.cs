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
    public class EntityTestClassDbEntity
    {
        public Guid Id { get; set; }

        public string StringProperty { get; set; }

        public int IntProperty { get; set; }

        public bool BoolProperty { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public decimal DecimalProperty { get; set; }

        public ulong UlongProperty { get; set; }

        public ICollection<string> ArrayProperty { get; set; }

        public int? OptionalProperty { get; set; }
    }
}