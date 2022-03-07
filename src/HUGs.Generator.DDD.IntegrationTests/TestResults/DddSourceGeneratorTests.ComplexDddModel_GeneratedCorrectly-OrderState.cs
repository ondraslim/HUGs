using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Enumerations
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
