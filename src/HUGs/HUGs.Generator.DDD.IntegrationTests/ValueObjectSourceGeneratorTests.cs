using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

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
            generatedFileTexts.Should().HaveCount(2);

            Check.CheckString(generatedFileTexts[0], checkName: fileName, fileExtension: "cs");
            Check.CheckString(generatedFileTexts[1], checkName: $"{fileName}DbEntity", fileExtension: "cs");
        }

        [Test]
        public void MultipleValidSchemas_GeneratedCorrectly()
        {
            var schema1 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple2.dddschema");
            var driver = SetupGeneratorDriver(new List<string> { schema1, schema2 });

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(4);

            Check.CheckString(generatedFileTexts[0], checkName: "SimpleValueObject1", fileExtension: "cs");
            Check.CheckString(generatedFileTexts[1], checkName: "SimpleValueObject1DbEntity", fileExtension: "cs");
            Check.CheckString(generatedFileTexts[2], checkName: "SimpleValueObject2", fileExtension: "cs");
            Check.CheckString(generatedFileTexts[3], checkName: "SimpleValueObject2DbEntity", fileExtension: "cs");
        }
    }
}
