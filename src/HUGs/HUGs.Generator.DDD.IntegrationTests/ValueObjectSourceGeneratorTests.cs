using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
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
        public void ValidSimpleValueObjectSchema_GeneratorRun_GeneratesCorrectValueObject(string valueObjectSchemaFile)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/ValueObjects/{valueObjectSchemaFile}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            Check.CheckString(generatedFileTexts.First(), checkName: valueObjectSchemaFile, fileExtension: "cs");
        }

        [Test]
        public void TwoValidSimpleValueObjectSchemas_GeneratorRun_GeneratesCorrectValueObjects()
        {
            var schema1 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple2.dddschema");
            var driver = SetupGeneratorDriver(new List<string> { schema1, schema2 });

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(2);

            Check.CheckString(generatedFileTexts.First(), checkName: "SimpleValueObject1", fileExtension: "cs");
            Check.CheckString(generatedFileTexts.Last(), checkName: "SimpleValueObject2", fileExtension: "cs");
        }

        [Test]
        public void ValidCompositeAddressValueObjectSchema_GeneratorRun_GeneratesCorrectValueObject()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/AddressValueObject.dddschema");
            var driver = SetupGeneratorDriver(schema);

            var compilationWithCountryId = CreateCompilation(@"
namespace Test
{
    public class Program 
    { 
        public static void Main() { } 
    }
}

public class CountryId 
{
    public System.Guid Id { get; set; }
}
");

            RunGenerator(driver, compilationWithCountryId, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            Check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }
    }
}
