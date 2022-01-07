using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class ValueObjectSourceGeneratorTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [TestCase("SimpleValueObject")]
        [TestCase("SimpleValueObject2")]
        [TestCase("SimpleValueObjectWithOptional")]
        public void SimpleSchema_GeneratedCorrectly(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/ValueObjects/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(3);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }

        [Test]
        public void MultipleValidSchemas_GeneratedCorrectly()
        {
            var schema1 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple2.dddschema");
            var driver = SetupGeneratorDriver(new List<string> { schema1, schema2 });

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(5);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }
    }
}
