using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class SimpleOptional : HUGs.Generator.DDD.BaseModels.ValueObject
    {
        public string Text { get; }

        public int Number { get; }

        public string? TextOptional { get; }

        public int? NumberOptional { get; }

        public SimpleOptional(string Text, int Number, string? TextOptional, int? NumberOptional)
        {
            this.Text = Text;
            this.Number = Number;
            this.TextOptional = TextOptional;
            this.NumberOptional = NumberOptional;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            yield return Number;
            yield return TextOptional;
            yield return NumberOptional;
        }

        partial void OnInitialized();
    }
}
