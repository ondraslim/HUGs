using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System;

namespace HUGs.Generator.DDD.Tests
{
    public class MapperGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        [TestCase(DddObjectKind.Aggregate)]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.ValueObject)]
        public void EmptySchema_MapperGeneratedCorrectly(DddObjectKind kind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = kind,
                Name = $"Simple{kind}",
                Properties = Array.Empty<DddObjectProperty>()
            };
            
            var actualCode = MapperGenerator.GenerateMapperCode(objectSchema, generatorConfiguration);
            
            check.CheckString(actualCode, checkName: TestHelper.GetGeneratedFileClass(actualCode), fileExtension: "cs");
        }

         // TODO: enum -> exception

        [Test]
        public void ComplexSchema_MapperGeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Aggregate,
                Name = "SimpleAggregate",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "SimpleString", Type = "string", ResolvedType = new DddPrimitiveType("string") },
                    new() { Name = "SimpleNumber", Type = "int", ResolvedType = new DddPrimitiveType("int") },
                    new() { Name = "SimpleComputed", Type = "string", Computed = true, ResolvedType = new DddPrimitiveType("string") },
                    new() { Name = "SimpleOptional", Type = "string?", ResolvedType = new DddPrimitiveType("string?") },
                    new() { Name = "SimpleCollection", Type = "int[]", ResolvedType = new DddCollectionType(new DddPrimitiveType("int")) },
                    new() { Name = "SimpleComputedCollection", Type = "int[]", Computed = true, ResolvedType = new DddCollectionType(new DddPrimitiveType("int")) },
                    new() { Name = "SimpleEntity", Type = "DddEntity", ResolvedType = new DddModelType("DddEntity", DddObjectKind.Entity) },
                    new() { Name = "SimpleValueObject", Type = "DddValueObject", ResolvedType = new DddModelType("DddValueObject", DddObjectKind.ValueObject) },
                    new() { Name = "SimpleEntityId", Type = "DddEntityId", ResolvedType = new DddIdType("DddEntity") },
                }
            };

            var actualCode = MapperGenerator.GenerateMapperCode(objectSchema, generatorConfiguration);
            
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}