using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using My.Desired.Namespace.Entities;
using My.Desired.Namespace.Aggregates;
using My.Desired.Namespace.ValueObjects;
using My.Desired.Namespace.Enumerations;
using My.Desired.Namespace.DbEntities;
using My.Desired.Namespace.Mappers;

namespace My.Desired.Namespace.ValueObjects
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
