using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class EnumerationSourceGeneratorTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [TestCase("EmptyEnumeration")]
        [TestCase("SimpleEnumeration")]
        [TestCase("ComplexEnumeration")]
        [TestCase("OrderStateEnumeration")]
        public void SimpleSchema_GeneratedCorrectly(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/Enumerations/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(2);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }
    }
}
