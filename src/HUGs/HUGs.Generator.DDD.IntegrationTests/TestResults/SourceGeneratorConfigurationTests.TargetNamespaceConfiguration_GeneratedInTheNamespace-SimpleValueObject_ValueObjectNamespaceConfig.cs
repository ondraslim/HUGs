using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using My.Desired.Namespace.ValueObjects;
using HUGs.DDD.Generated.Enumeration;

namespace My.Desired.Namespace.ValueObjects
{
    public partial class Simple1 : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public string Text { get; }

        public Simple1(string Text)
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
