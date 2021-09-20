using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Playground.Mocks;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using HUGs.Generator.DDD;
using HUGs.Generator.DDD.Common;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Playground
{
    internal class Program
    {
        private static void Main()
        {
            //TryYamlDeserialize();

            //TryRunGeneratorWithoutDependencies();

            TryRunGenerator();
        }

        private static void TryRunGenerator()
        {
            var inputCompilation = CreateCompilation(@"
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

            var generator = new Generator();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new List<ISourceGenerator> { generator });

            var text = File.ReadAllText("../../../AddressValueObject.dddschema");
            var additionalFile = new TestAdditionalText(text: text, path: "dummy.dddschema");
            var additionalFiles = ImmutableArray.Create<AdditionalText>(additionalFile);

            driver = driver.AddAdditionalTexts(additionalFiles);
            
            // Run the generation pass
            driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            static Compilation CreateCompilation(string source)
                => CSharpCompilation.Create("compilation",
                    new[] { CSharpSyntaxTree.ParseText(source) },
                    new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                    new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }

        private static void TryRunGeneratorWithoutDependencies()
        {
            var generator = new Generator();
            
            generator.Initialize(new GeneratorInitializationContext());
            generator.Execute(new GeneratorExecutionContext());
        }

        private static void TryYamlDeserialize()
        {
            var yaml = File.ReadAllText("../../../AddressValueObject.dddschema");
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var schema = deserializer.Deserialize<DddObjectSchema>(yaml);
        }
    }
}
