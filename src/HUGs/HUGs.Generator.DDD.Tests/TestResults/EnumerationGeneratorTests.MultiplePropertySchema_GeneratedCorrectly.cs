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
    public class OrderState2 : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly OrderState2 Created = new OrderState2(nameof(Created), "Created", 1);
        public static readonly OrderState2 Canceled = new OrderState2(nameof(Canceled), "Canceled", 42);
        public string Name { get; }

        public int Count { get; }

        private OrderState2(string internalName, string Name, int Count): base(internalName)
        {
            this.Name = Name;
            this.Count = Count;
        }
    }
}
