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
        [TestCase("OrderAggregate")]
        public void ValidSimpleAggregateSchema_GeneratorRun_GeneratesCorrectAggregate(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/Aggregates/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            Check.CheckString(generatedFileTexts.First(), checkName: fileName, fileExtension: "cs");
        }
    }
}