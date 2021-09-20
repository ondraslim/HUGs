using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Tests.Mocks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HUGs.Generator.DDD.Tests
{
    public class DddSourceGeneratorOfValueObjectTests
    {
        private Compilation emptyInputCompilation;
        private readonly OutputChecker check = new("TestResults/ValueObjects");

        [SetUp]
        public void Setup()
        {
            emptyInputCompilation = CreateCompilation(@"class Program { static void Main() {} }");
        }

        [Test]
        public void ValidSimpleValueObjectSchema_GeneratorRun_GeneratesCorrectValueObject()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var driver = SetupGeneratorDriver(schema);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        [Test]
        public void ValidSimpleValueObjectSchema2_GeneratorRun_GeneratesCorrectValueObject()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject2.dddschema");
            var driver = SetupGeneratorDriver(schema);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        [Test]
        public void TwoValidSimpleValueObjectSchemas_GeneratorRun_GeneratesCorrectValueObjects()
        {
            var schema1 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectMultiple2.dddschema");
            var driver = SetupGeneratorDriver(new List<string> { schema1, schema2 });

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(2);
            check.CheckString(generatedFileTexts.First(), checkName: "SimpleValueObject1", fileExtension: "cs");
            check.CheckString(generatedFileTexts.Last(), checkName: "SimpleValueObject2", fileExtension: "cs");
        }

        [Test]
        public void ValidSimpleValueObjectWithOptionalsSchema_GeneratorRun_GeneratesCorrectValueObject()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObjectWithOptional.dddschema");
            var driver = SetupGeneratorDriver(schema);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
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

namespace Test.Country 
{
    public class CountryId 
    {
        public System.Guid Id { get; set; }
    }
}");

            driver.RunGeneratorsAndUpdateCompilation(compilationWithCountryId, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !compilationWithCountryId.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        private static GeneratorDriver SetupGeneratorDriver(string schema)
            => SetupGeneratorDriver(new List<string> { schema });

        private static GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas)
        {
            var generator = new Generator();

            return CSharpGeneratorDriver.Create(
                new List<ISourceGenerator> { generator },
                schemas.Select(s => new TestAdditionalText(text: s, path: "dummyPath.dddschema")));
        }

        private static Compilation CreateCompilation(string source)
        {
            return CSharpCompilation.Create(
                "compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}
