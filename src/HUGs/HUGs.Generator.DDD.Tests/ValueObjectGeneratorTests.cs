using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class ValueObjectGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void NoPropertySchema_GeneratedCorrectly()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleClass1",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void SinglePropertySchema_GeneratedCorrectly()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[] { new() { Name = "Number", Optional = false, Type = "int" } }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void ComputedPropertySchema_GeneratedCorrectly()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Optional = false, Type = "int" },
                    new() { Name = "ComputedNumber", Computed = true, Type = "int" }
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void MultiplePropertiesSchema_GeneratedCorrectly()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "MultiplePropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Optional = false, Type = "int" },
                    new() { Name = "Number2", Optional = true, Type = "int" },
                    new() { Name = "Text", Optional = false, Type = "string" },
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}