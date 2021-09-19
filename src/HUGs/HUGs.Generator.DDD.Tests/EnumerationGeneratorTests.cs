using FluentAssertions;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.Tests.Tools.Extensions;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class EnumerationGeneratorTests
    {
        [Test]
        public void GivenEmptyEnumerationSchema_CorrectlyGeneratesEnumerationClass()
        {
            var inputEnumerationObject = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "SimpleClass",
                Properties = new DddObjectProperty[] { },
                Values = new DddObjectValue[] { }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{{
    public class {inputEnumerationObject.Name} : Enumeration
    {{
        private {inputEnumerationObject.Name}(string internalName): base(internalName)
        {{
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        public void GivenEnumerationSchemaWithSingleProperty_CorrectlyGeneratesEnumerationClass()
        {
            var inputEnumerationObject = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "OrderState",
                Properties = new DddObjectProperty[] { new() { Name = "Name", Type = "string" } },
                Values = new DddObjectValue[]
                {
                    new()
                    {
                        Name = "Created",
                        PropertyInitialization = new DddPropertyInitialization[] { new() { PropertyName = "Name", PropertyValue = "Vytvořeno" } }
                    },
                    new()
                    {
                        Name = "Canceled",
                        PropertyInitialization = new DddPropertyInitialization[] { new() { PropertyName = "Name", PropertyValue = "Zrušeno" } }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{{
    public class {inputEnumerationObject.Name} : Enumeration
    {{
        public static readonly {inputEnumerationObject.Name} Created = new {inputEnumerationObject.Name}(nameof(Created), ""Vytvořeno"");
        public static readonly {inputEnumerationObject.Name} Canceled = new {inputEnumerationObject.Name}(nameof(Canceled), ""Zrušeno"");
        public string Name {{ get; }}

        private {inputEnumerationObject.Name}(string internalName, string Name): base(internalName)
        {{
            this.Name = Name;
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }
        
        [Test]
        public void GivenEnumerationSchemaWithMultipleProperties_CorrectlyGeneratesEnumerationClass()
        {
            var inputEnumerationObject = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "OrderState",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Name", Type = "string" },
                    new() { Name = "Count", Type = "int" }
                },
                Values = new DddObjectValue[]
                {
                    new()
                    {
                        Name = "Created",
                        PropertyInitialization = new DddPropertyInitialization[]
                        {
                            new() { PropertyName = "Name", PropertyValue = "Vytvořeno" },
                            new() { PropertyName = "Count", PropertyValue = "1" }
                        }
                    },
                    new()
                    {
                        Name = "Canceled",
                        PropertyInitialization = new DddPropertyInitialization[]
                        {
                            new() { PropertyName = "Name", PropertyValue = "Zrušeno" },
                            new() { PropertyName = "Count", PropertyValue = "42" }
                        }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject);
            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.Enumeration
{{
    public class {inputEnumerationObject.Name} : Enumeration
    {{
        public static readonly {inputEnumerationObject.Name} Created = new {inputEnumerationObject.Name}(nameof(Created), ""Vytvořeno"", 1);
        public static readonly {inputEnumerationObject.Name} Canceled = new {inputEnumerationObject.Name}(nameof(Canceled), ""Zrušeno"", 42);
        public string Name {{ get; }}

        public int Count {{ get; }}

        private {inputEnumerationObject.Name}(string internalName, string Name, int Count): base(internalName)
        {{
            this.Name = Name;
            this.Count = Count;
        }}
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }
    }
}