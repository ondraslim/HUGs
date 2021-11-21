using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System;

namespace HUGs.Generator.DDD.Tests
{
    public class DbEntityGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void AllowedKindWithWhitelistedTypes_DbEntityGenerated()
        {
            var properties = new[] {
                new DddObjectProperty { Name = "StringProperty", Type = "string" },
                new DddObjectProperty { Name = "IntProperty", Type = "int" },
                new DddObjectProperty { Name = "BoolProperty", Type = "bool" },
                new DddObjectProperty { Name = "DateTimeProperty", Type = "DateTime" },
                new DddObjectProperty { Name = "DecimalProperty", Type = "decimal" },
                new DddObjectProperty { Name = "UlongProperty", Type = "ulong" },
                new DddObjectProperty { Name = "ArrayProperty", Type = "string[]" },
                new DddObjectProperty { Name = "ComputedProperty", Type = "int", Computed = true },
                new DddObjectProperty { Name = "OptionalProperty", Type = "int", Optional = true },
            };

            foreach (var kind in new[] { DddObjectKind.Aggregate, DddObjectKind.Entity, DddObjectKind.ValueObject })
            {
                var schema = new DddObjectSchema
                {
                    Kind = kind,
                    Name = $"{kind}TestClass",
                    Properties = properties
                };

                var model = new DddModel();
                model.AddObjectSchema(schema);

                var actualCode = DbEntityGenerator.GenerateDbEntity(schema, generatorConfiguration, model);
                check.CheckString(actualCode, checkName: TestHelper.GetGeneratedFileClass(actualCode), fileExtension: "cs");
            }
        }

        [Test]
        public void NotAllowedKind_DbEntityNotGenerated()
        {
            var schema = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "TestClassName"
            };

            var model = new DddModel();
            model.AddObjectSchema(schema);
            Action act = () => DbEntityGenerator.GenerateDbEntity(schema, generatorConfiguration, model);

            act.Should().ThrowExactly<DddSchemaKindDbEntityNotSupportedException>();
        }
    }
}