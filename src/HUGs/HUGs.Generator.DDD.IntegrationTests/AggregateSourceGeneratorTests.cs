using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class AggregateSourceGeneratorTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [TestCase("SimpleAggregate")]
        public void SimpleSchema_GeneratedCorrectly(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/Aggregates/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(2);

            Check.CheckString(generatedFileTexts[0], checkName: fileName, fileExtension: "cs");
            Check.CheckString(generatedFileTexts[1], checkName: $"{fileName}DbEntity", fileExtension: "cs");
        }
    }
}
