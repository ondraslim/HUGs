using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class Simple3 : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
    {
        public string Text { get; }

        public Simple3(string Text)
        {
            this.Text = Text;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
        }

        partial void OnInitialized();
    }
}
