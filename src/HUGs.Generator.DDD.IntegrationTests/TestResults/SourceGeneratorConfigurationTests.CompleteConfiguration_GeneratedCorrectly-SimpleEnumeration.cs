using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Additional.Using1;
using My.Additional.Using2;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;

namespace My.Desired.Namespace.Enumerations
{
    public class SimpleEnumeration : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly SimpleEnumeration SampleEnumeration = new(nameof(SampleEnumeration), "PropertyNameValue");

        public string Name { get; }

        private SimpleEnumeration(string internalName, string Name)
        	: base(internalName)
        {
            this.Name = Name;
        }

        public static SimpleEnumeration FromString(string name)
        {
            return name switch
            {
            	"SampleEnumeration" => SampleEnumeration,
            	_ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
