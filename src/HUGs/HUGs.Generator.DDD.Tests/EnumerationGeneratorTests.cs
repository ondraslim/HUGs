using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace HUGs.Generator.DDD.Tests
{
    public class EnumerationGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void EmptySchema_GeneratedCorrectly()
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
        public void SinglePropertySchema_GeneratedCorrectly()
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
                        Properties = new Dictionary<string, string>{ { "Name", "Created" } }
                    },
                    new()
                    {
                        Name = "Canceled",
                        Properties = new Dictionary<string, string> { { "Name", "Canceled" } }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
        
        [Test]
        public void MultiplePropertySchema_GeneratedCorrectly()
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
                        Properties = new Dictionary<string, string>
                        {
                            { "Name", "Created" },
                            { "Count", "1" },
                        }
                    },
                    new()
                    {
                        Name = "Canceled",
                        Properties = new Dictionary<string, string>
                        {
                            { "Name", "Canceled" },
                            { "Count", "42" },
                        }
                    }
                }
            };

            var actualCode = EnumerationGenerator.GenerateEnumerationCode(inputEnumerationObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}