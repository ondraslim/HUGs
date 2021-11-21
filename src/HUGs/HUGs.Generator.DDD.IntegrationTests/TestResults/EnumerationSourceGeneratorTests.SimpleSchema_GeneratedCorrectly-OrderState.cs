using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly OrderState Created = new(nameof(Created), "Created");
        public static readonly OrderState Canceled = new(nameof(Canceled), "Canceled");
        public string Name { get; }

        private OrderState(string internalName, string Name): base(internalName)
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
