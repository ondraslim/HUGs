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
    public partial class Simple2 : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
    {
        public string Text { get; }

        public int Number { get; }

        public Simple2(string Text, int Number)
        {
            this.Text = Text;
            this.Number = Number;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return Number;
        }

        partial void OnInitialized();
    }
}
