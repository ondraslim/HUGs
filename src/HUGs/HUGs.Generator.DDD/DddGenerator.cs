using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HUGs.Generator.DDD
{
    [Generator]
    public class DddGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // not needed
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var dddSchemas = GetDddSchemaFiles(context);

            var dddModel = BuildDddModel(dddSchemas);

            GenerateDddModelSource(context, dddModel);
        }

        private static void GenerateDddModelSource(GeneratorExecutionContext context, DddModel dddModel)
        {
            foreach (var valueObject  in dddModel.ValueObjects)
            {
                AddValueObjectSource(context, valueObject);
            }
        }

        private static void AddValueObjectSource(GeneratorExecutionContext context, DddObjectSchema valueObject)
        {
            // TODO: add custom usings
            var sb = new StringBuilder($@"using System;
using System.Collections.Generic;

namespace HUGs.DDD.Generated.ValueObject
{{
    public partial class {valueObject.Name} : HUGs.Generator.DDD.Common.DDD.Base.ValueObject
    {{");

            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                var model = context.Compilation.GetSemanticModel(tree);
                var knownTypes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

                var classDeclarationSyntax = knownTypes.FirstOrDefault(n => n.Identifier.ValueText == "CountryId");
                var typeInfo = model.GetTypeInfo(classDeclarationSyntax);
                if (classDeclarationSyntax != default)
                {
                    var ns = typeInfo.Type?.ContainingNamespace;
                }
            }

            var properties = valueObject.Properties.Select(p => $"{p.Type}{(p.Optional ? "?" : "")} {p.Name}").ToList();
            
            foreach (var property in properties)
            {
                sb.Append($@"
        public {property} {{ get; }}");
            }

            sb.AppendLine();

            sb.Append($@"
        public {valueObject.Name}({string.Join(", ", properties)})
        {{");

            foreach (var property in valueObject.Properties)
            {
                sb.Append($@"
            this.{property.Name} = {property.Name};");
            }

            sb.Append(@"
        }");

            sb.AppendLine();

            sb.Append(@"
        protected override IEnumerable<object> GetAtomicValues()
        {");

            foreach (var property in valueObject.Properties)
            {
                sb.Append($@"
            yield return {property.Name};");
            }


            sb.AppendLine(@"
        }
    }
}");

            context.AddSource($"{valueObject.Name}ValueObject", sb.ToString());
        }

        private static IEnumerable<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddschema", StringComparison.OrdinalIgnoreCase));
        }

        private static DddModel BuildDddModel(IEnumerable<AdditionalText> schemaFiles)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var dddModel = new DddModel();
            foreach (var schemaFile in schemaFiles)
            {
                var schemaText = schemaFile.GetText()?.ToString();

                if (string.IsNullOrWhiteSpace(schemaText)) continue;

                var schema = deserializer.Deserialize<DddObjectSchema>(schemaText);
                if (schema is not null)
                {
                    dddModel.AddValueObjectSchema(schema);
                }
            }

            return dddModel;
        }
    }
}
