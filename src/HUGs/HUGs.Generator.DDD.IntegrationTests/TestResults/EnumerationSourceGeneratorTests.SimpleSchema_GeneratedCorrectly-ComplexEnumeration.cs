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
    public class ComplexEnumeration : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly ComplexEnumeration SampleEnumeration1 = new(nameof(SampleEnumeration1), "PropertyNameValue1", 1);
        public static readonly ComplexEnumeration SampleEnumeration2 = new(nameof(SampleEnumeration2), "PropertyNameValue2", 2);
        public static readonly ComplexEnumeration SampleEnumeration3 = new(nameof(SampleEnumeration3), "PropertyNameValue3", 3);
        public string Name { get; }

        public int Amount { get; }

        private ComplexEnumeration(string internalName, string Name, int Amount): base(internalName)
        {
            this.Name = Name;
            this.Amount = Amount;
        }

        public static ComplexEnumeration FromString(string name)
        {
            return name switch
            {
            	"SampleEnumeration1" => SampleEnumeration1,
            	"SampleEnumeration2" => SampleEnumeration2,
            	"SampleEnumeration3" => SampleEnumeration3,
            	_ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
