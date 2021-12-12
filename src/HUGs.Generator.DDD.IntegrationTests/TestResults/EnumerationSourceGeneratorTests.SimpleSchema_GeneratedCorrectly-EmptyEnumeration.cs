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
    public class EmptyEnumeration : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly EmptyEnumeration SampleEmptyEnumeration = new(nameof(SampleEmptyEnumeration));

        private EmptyEnumeration(string internalName)
        	: base(internalName)
        {
        }

        public static EmptyEnumeration FromString(string name)
        {
            return name switch
            {
            	"SampleEmptyEnumeration" => SampleEmptyEnumeration,
            	_ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
