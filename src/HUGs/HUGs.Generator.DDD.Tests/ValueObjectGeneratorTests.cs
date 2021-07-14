using FluentAssertions;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.Tests.Tools.Extensions;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class ValueObjectGeneratorTests
    {
        [Test]
        public void GivenEmptyValueObjectSchema_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = "ValueObject",
                Name = "SimpleClass",
                Properties = new Property[] { }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.ValueObject
{{
    public partial class {inputValueObject.Name} : HUGs.Generator.DDD.Common.DDD.Base.ValueObject
    {{
        public {inputValueObject.Name}()
        {{
        }}

        protected override IEnumerable<object> GetAtomicValues()
        {{
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        public void GivenValueObjectSchemaWithSingleProperty_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = "ValueObject",
                Name = "SimpleClass",
                Properties = new Property[] { new() { Name = "Number", Optional = false, Type = "int" } }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.ValueObject
{{
    public partial class {inputValueObject.Name} : HUGs.Generator.DDD.Common.DDD.Base.ValueObject
    {{
        public int Number {{ get; }}

        public {inputValueObject.Name}(int Number)
        {{
            this.Number = Number;
        }}

        protected override IEnumerable<object> GetAtomicValues()
        {{
            yield return Number;
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        public void GivenValueObjectSchemaWithMutipleProperties_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = "ValueObject",
                Name = "SimpleClass",
                Properties = new Property[]
                {
                    new() { Name = "Number", Optional = false, Type = "int" },
                    new() { Name = "Number2", Optional = true, Type = "int" },
                    new() { Name = "Text", Optional = false, Type = "string" },
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.ValueObject
{{
    public partial class {inputValueObject.Name} : HUGs.Generator.DDD.Common.DDD.Base.ValueObject
    {{
        public int Number {{ get; }}

        public int? Number2 {{ get; }}

        public string Text {{ get; }}

        public {inputValueObject.Name}(int Number, int? Number2, string Text)
        {{
            this.Number = Number;
            this.Number2 = Number2;
            this.Text = Text;
        }}

        protected override IEnumerable<object> GetAtomicValues()
        {{
            yield return Number;
            yield return Number2;
            yield return Text;
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }
    }
}