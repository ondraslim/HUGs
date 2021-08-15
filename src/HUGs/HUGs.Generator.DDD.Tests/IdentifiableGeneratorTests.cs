using FluentAssertions;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.Tests.Tools.Extensions;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class IdentifiableGeneratorTests
    {
        [Test]
        [TestCase("Entity")]
        [TestCase("Aggregate")]
        public void GivenEmptyIdentifiableSchema_CorrectlyGeneratesIdentifiableClasses(string identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"Simple{identifiableKind}",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = objectSchema.IsEntitySchema ?
                IdentifiableGenerator.GenerateEntityCode(objectSchema)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema);

            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.{identifiableKind}
{{
    public class {objectSchema.Name}Id : EntityId<{objectSchema.Name}>
    {{
        public {objectSchema.Name}Id(string value)
        {{
        }}
    }}

    public partial class {objectSchema.Name} : {identifiableKind}<{objectSchema.Name}Id>
    {{
        public {objectSchema.Name}({objectSchema.Name}Id id): base(id)
        {{
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        [TestCase("Entity")]
        [TestCase("Aggregate")]
        public void GivenIdentifiableSchemaWithProperties_CorrectlyGeneratesIdentifiableClasses(string identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"Properties{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = objectSchema.IsEntitySchema ?
                IdentifiableGenerator.GenerateEntityCode(objectSchema)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema);

            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.{identifiableKind}
{{
    public class {objectSchema.Name}Id : EntityId<{objectSchema.Name}>
    {{
        public {objectSchema.Name}Id(string value)
        {{
        }}
    }}

    public partial class {objectSchema.Name} : {identifiableKind}<{objectSchema.Name}Id>
    {{
        public string Text {{ get; private set; }}

        public double? Number {{ get; private set; }}

        public {objectSchema.Name}({objectSchema.Name}Id id, string Text, double? Number): base(id)
        {{
            this.Text = Text;
            this.Number = Number;
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        [TestCase("Entity")]
        [TestCase("Aggregate")]
        public void GivenIdentifiableSchemaWithArrayProperty_CorrectlyGeneratesIdentifiableClasses(string identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"ArrayProperty{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Items", Type = "OrderItem[]" },
                }
            };

            var actualCode = objectSchema.IsEntitySchema ?
                IdentifiableGenerator.GenerateEntityCode(objectSchema)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema);

            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.{identifiableKind}
{{
    public class {objectSchema.Name}Id : EntityId<{objectSchema.Name}>
    {{
        public {objectSchema.Name}Id(string value)
        {{
        }}
    }}

    public partial class {objectSchema.Name} : {identifiableKind}<{objectSchema.Name}Id>
    {{
        private List<OrderItem> _Items;
        public IReadOnlyList<OrderItem> Items => _Items;
        public {objectSchema.Name}({objectSchema.Name}Id id, IReadOnlyList<OrderItem> Items): base(id)
        {{
            this._Items = Items;
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        [TestCase("Entity")]
        [TestCase("Aggregate")]
        public void GivenIdentifiableSchemaWithVariousProperties_CorrectlyGeneratesIdentifiableClasses(string identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"ArrayProperty{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Items", Type = "OrderItem[]" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = objectSchema.IsEntitySchema ?
                IdentifiableGenerator.GenerateEntityCode(objectSchema)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema);

            var expectedCode = $@"using System;
using System.Collections.Generic;
using HUGs.Generator.DDD.BaseModels;

namespace HUGs.DDD.Generated.{identifiableKind}
{{
    public class {objectSchema.Name}Id : EntityId<{objectSchema.Name}>
    {{
        public {objectSchema.Name}Id(string value)
        {{
        }}
    }}

    public partial class {objectSchema.Name} : {identifiableKind}<{objectSchema.Name}Id>
    {{
        private List<OrderItem> _Items;
        public string Text {{ get; private set; }}

        public IReadOnlyList<OrderItem> Items => _Items;
        public double? Number {{ get; private set; }}

        public {objectSchema.Name}({objectSchema.Name}Id id, string Text, IReadOnlyList<OrderItem> Items, double? Number): base(id)
        {{
            this.Text = Text;
            this._Items = Items;
            this.Number = Number;
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }
    }
}