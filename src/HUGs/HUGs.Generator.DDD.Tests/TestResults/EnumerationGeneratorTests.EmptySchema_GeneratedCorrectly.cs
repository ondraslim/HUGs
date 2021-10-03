using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Enumeration
{
    public class SimpleClass1 : HUGs.Generator.DDD.BaseModels.Enumeration
    {
        private SimpleClass1(string internalName): base(internalName)
        {
        }
    }
}
