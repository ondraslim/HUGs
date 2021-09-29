using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class EnumerationGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults/Enumeration");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void GivenEmptyEnumerationSchema_CorrectlyGeneratesEnumerationClass()
        {
            var inputEnumerationObject = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "SimpleClass1",
                Properties = new DddObjectProperty[] { },
                Values = new DddObjectValue[] { }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
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
                        Properties = new DddPropertyInitialization[] { new() { Property = "Name", Value = "Vytvořeno" } }
                    },
                    new()
                    {
                        Name = "Canceled",
                        Properties = new DddPropertyInitialization[] { new() { Property = "Name", Value = "Zrušeno" } }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
        
        [Test]
        public void GivenEnumerationSchemaWithMultipleProperties_CorrectlyGeneratesEnumerationClass()
        {
            var inputEnumerationObject = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "OrderState2",
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
                        Properties = new DddPropertyInitialization[]
                        {
                            new() { Property = "Name", Value = "Vytvořeno" },
                            new() { Property = "Count", Value = "1" }
                        }
                    },
                    new()
                    {
                        Name = "Canceled",
                        Properties = new DddPropertyInitialization[]
                        {
                            new() { Property = "Name", Value = "Zrušeno" },
                            new() { Property = "Count", Value = "42" }
                        }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}