using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class EntitySourceGeneratorTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [TestCase("SimpleEntity")]
        public void ValidEntitySchema_GeneratorRun_GeneratesCorrectEntity(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/Entities/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            Check.CheckString(generatedFileTexts.First(), checkName: fileName, fileExtension: "cs");
        }
    }
}
