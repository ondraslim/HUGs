using CheckTestOutput;
using HUGs.Generator.DDD.Common;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class EnumerationGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults/Enumeration/Generator");

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

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject);
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
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}