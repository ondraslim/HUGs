using Hugs.Generator.DDD;
using HUGs.Generator.DDD.Tests.Mocks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HUGs.Generator.DDD.Tests
{
    public class ValueObjectTests
    {
        private Compilation emptyInputCompilation;

        [SetUp]
        public void Setup()
        {
            emptyInputCompilation = CreateCompilation(@"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
");
        }

        [Test]
        public void Test1()
        {
            var schema = File.ReadAllText("../../../TestData/AddressValueObject.dddschema");
            var driver = SetupGeneratorDriver(schema);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);
        }

        private static GeneratorDriver SetupGeneratorDriver(string schema) 
            => SetupGeneratorDriver(new List<string> { schema });

        private static GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas)
        {
            var generator = new DddGenerator();

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
