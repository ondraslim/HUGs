﻿using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;
using HUGs.DDD.Generated.DbEntity;
using HUGs.DDD.Generated.Mapper;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly OrderState Created = new(nameof(Created), "Created");

        public static readonly OrderState Canceled = new(nameof(Canceled), "Canceled");

        public string Name { get; }

        private OrderState(string internalName, string Name)
            : base(internalName)
        {
            this.Name = Name;
        }

        public static OrderState FromString(string name)
        {
            return name switch
            {
                "Created" => Created,
                "Canceled" => Canceled,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

namespace HUGs.DDD.Generated.Entity
{
}

namespace HUGs.DDD.Generated.Aggregate
{
}

namespace HUGs.DDD.Generated.ValueObject
{
}

namespace HUGs.DDD.Generated.Enumeration
{
}