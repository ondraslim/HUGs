using FluentAssertions;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.Tests.Tools.Extensions;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class EntityGeneratorTests
    {
        [Test]
        public void GivenEmptyEntitySchema_CorrectlyGeneratesEntityClasses()
        {
            var inputEntity = new DddObjectSchema
            {
                Kind = "Entity",
                Name = "SimpleEntity",
                Properties = new Property[] { }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(inputEntity);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.Entity
{{
    public class {inputEntity.Name}Id : EntityId<{inputEntity.Name}>
    {{
        public {inputEntity.Name}Id(string value)
        {{
        }}
    }}

    public partial class {inputEntity.Name} : Aggregate<{inputEntity.Name}Id>
    {{
        public {inputEntity.Name}(string value): base(value)
        {{
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        public void GivenEntitySchemaWithProperties_CorrectlyGeneratesEntityClasses()
        {
            var inputEntity = new DddObjectSchema
            {
                Kind = "Entity",
                Name = "PropertiesEntity",
                Properties = new Property[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(inputEntity);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.Entity
{{
    public class {inputEntity.Name}Id : EntityId<{inputEntity.Name}>
    {{
        public {inputEntity.Name}Id(string value)
        {{
        }}
    }}

    public partial class {inputEntity.Name} : Aggregate<{inputEntity.Name}Id>
    {{
        public string Text {{ get; private set; }}

        public double? Number {{ get; private set; }}

        public {inputEntity.Name}(string value, string Text, double? Number): base(value)
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
        public void GivenEntitySchemaWithArrayProperty_CorrectlyGeneratesEntityClasses()
        {
            var inputEntity = new DddObjectSchema
            {
                Kind = "Entity",
                Name = "ArrayPropertyEntity",
                Properties = new Property[]
                {
                    new() { Name = "Items", Type = "OrderItem[]" },
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(inputEntity);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.Entity
{{
    public class {inputEntity.Name}Id : EntityId<{inputEntity.Name}>
    {{
        public {inputEntity.Name}Id(string value)
        {{
        }}
    }}

    public partial class {inputEntity.Name} : Aggregate<{inputEntity.Name}Id>
    {{
        private List<OrderItem> _Items;
        public IReadOnlyList<OrderItem> Items => _Items;
        public {inputEntity.Name}(string value, IReadOnlyList<OrderItem> Items): base(value)
        {{
            this._Items = Items;
        }}

        private partial void OnInitialized();
    }}
}}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }

        [Test]
        public void GivenEntitySchemaWithVariousProperties_CorrectlyGeneratesEntityClasses()
        {
            var inputEntity = new DddObjectSchema
            {
                Kind = "Entity",
                Name = "ArrayPropertyEntity",
                Properties = new Property[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Items", Type = "OrderItem[]" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(inputEntity);
            var expectedCode = $@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.Entity
{{
    public class {inputEntity.Name}Id : EntityId<{inputEntity.Name}>
    {{
        public {inputEntity.Name}Id(string value)
        {{
        }}
    }}

    public partial class {inputEntity.Name} : Aggregate<{inputEntity.Name}Id>
    {{
        private List<OrderItem> _Items;
        public string Text {{ get; private set; }}

        public IReadOnlyList<OrderItem> Items => _Items;
        public double? Number {{ get; private set; }}

        public {inputEntity.Name}(string value, string Text, IReadOnlyList<OrderItem> Items, double? Number): base(value)
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