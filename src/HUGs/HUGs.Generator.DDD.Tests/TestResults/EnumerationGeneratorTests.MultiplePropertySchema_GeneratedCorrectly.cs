using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState2 : HUGs.Generator.DDD.BaseModels.Enumeration
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
