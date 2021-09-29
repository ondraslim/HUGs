using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        public static readonly OrderState Created = new OrderState(nameof(Created), "Created");
        public static readonly OrderState Canceled = new OrderState(nameof(Canceled), "Canceled");
        public string Name { get; }

        private OrderState(string internalName, string Name): base(internalName)
        {
            this.Name = Name;
        }
    }
}
