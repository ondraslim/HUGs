using CheckTestOutput;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Ddd;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class ValueObjectGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults/ValueObjects/Generator");

        [Test]
        public void GivenEmptyValueObjectSchema_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleClass1",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void GivenValueObjectSchemaWithSingleProperty_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[] { new() { Name = "Number", Optional = false, Type = "int" } }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void GivenValueObjectSchemaWithMultipleProperties_CorrectlyGeneratesValueObjectClass()
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

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}