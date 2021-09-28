using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using My.Additional.Using1;
using My.Additional.Using2;

namespace My.Desired.Namespace.ValueObjects
{
    public partial class Simple1 : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public string Text { get; }

        public Simple1(string Text)
        {
            this.Text = Text;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
        }
    }
}
