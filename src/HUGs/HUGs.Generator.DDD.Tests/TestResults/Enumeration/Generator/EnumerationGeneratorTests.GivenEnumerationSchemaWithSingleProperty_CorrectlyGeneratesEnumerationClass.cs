using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState : Enumeration
    {
        public static readonly OrderState Created = new OrderState(nameof(Created), "Vytvořeno");
        public static readonly OrderState Canceled = new OrderState(nameof(Canceled), "Zrušeno");
        public string Name { get; }

        private OrderState(string internalName, string Name): base(internalName)
        {
            this.Name = Name;
        }
    }
}
