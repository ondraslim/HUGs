using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
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
                new DddObjectProperty { Name = "StringProperty", Type = "string", ResolvedType = new DddPrimitiveType("string") },
                new DddObjectProperty { Name = "IntProperty", Type = "int", ResolvedType = new DddPrimitiveType("int") },
                new DddObjectProperty { Name = "BoolProperty", Type = "bool", ResolvedType = new DddPrimitiveType("bool") },
                new DddObjectProperty { Name = "DateTimeProperty", Type = "DateTime", ResolvedType = new DddPrimitiveType("DateTime") },
                new DddObjectProperty { Name = "DecimalProperty", Type = "decimal", ResolvedType = new DddPrimitiveType("decimal") },
                new DddObjectProperty { Name = "UlongProperty", Type = "ulong", ResolvedType = new DddPrimitiveType("ulong") },
                new DddObjectProperty { Name = "ArrayProperty", Type = "string[]", ResolvedType = new DddCollectionType(new DddPrimitiveType("string")) },
                new DddObjectProperty { Name = "ComputedProperty", Type = "int", Computed = true, ResolvedType = new DddPrimitiveType("int") },
                new DddObjectProperty { Name = "OptionalProperty", Type = "int?", ResolvedType = new DddPrimitiveType("int?") },
            };

            foreach (var kind in new[] { DddObjectKind.Aggregate, DddObjectKind.Entity, DddObjectKind.ValueObject })
            {
                var schema = new DddObjectSchema
                {
                    Kind = kind,
                    Name = $"{kind}TestClass",
                    Properties = properties
                };

                var actualCode = DbEntityGenerator.GenerateDbEntity(schema, generatorConfiguration);
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
            
            Action act = () => DbEntityGenerator.GenerateDbEntity(schema, generatorConfiguration);

            act.Should().ThrowExactly<DddSchemaKindDbEntityNotSupportedException>();
        }
    }
}