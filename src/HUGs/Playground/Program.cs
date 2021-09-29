using HUGs.Generator.DDD;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Playground.Mocks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Playground
{
    internal class Program
    {
        private static void Main()
        {
            var isValidIdentifier = SyntaxFacts.IsValidIdentifier("{asd");
            //TryRunGenerator();

            //TrySerialize();

            //TryDeserialize();
        }

        private static void TryDeserialize()
        {
            var enumeration = File.ReadAllText("../../../SimpleEnumeration.dddschema");
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(NullNamingConvention.Instance)
                .Build();

            var schema = deserializer.Deserialize<DddObjectSchema>(enumeration);
        }

        private static void TrySerialize()
        {
            var enumeration = new DddObjectSchema
            {
                Kind = DddObjectKind.Enumeration,
                Name = "SimpleEnumeration",
                Properties = new[]
                {
                    new DddObjectProperty {Name = "Name", Type = "string"}
                },
                Values = new[]
                {
                    new DddObjectValue
                    {
                        Name = "SimpleEnumerationExample",
                        Properties = new Dictionary<string, string>{ { "Name", "NameValue" } }
                    }
                }
            };

            var serializer = new SerializerBuilder()
                .WithNamingConvention(NullNamingConvention.Instance)
                .Build();
            var text = serializer.Serialize(enumeration);
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
    }
}
