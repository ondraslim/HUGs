using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hugs.Generator.DDD
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
            //var sb = new StringBuilder(@"");
            // inject the created source into the users compilation
            //context.AddSource("HelloWorldGenerated", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private void GenerateDddModelSource(GeneratorExecutionContext context, DddModel dddModel)
        {
            foreach (var valueObject  in dddModel.ValueObjects)
            {
                AddValueObjectSource(context, valueObject);
            }
        }

        private void AddValueObjectSource(GeneratorExecutionContext context, DddObjectSchema valueObject)
        {
            var sb = new StringBuilder($@"using System;
using HUGs.Generator.DDD.Common.DDD.Base;

namespace Hugs.DDD.Generated.ValueObject
{{
    public partial class {valueObject.Name} : ValueObject
    {{");
            var properties = valueObject.Properties.Select(p => $"{p.Type}{(p.Optional ? "?" : "")} {p.Name}");

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


            sb.AppendLine(@"
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

                // TODO: validate schema
                // TODO: add to specific type
                if (schema is not null)
                {
                    dddModel.AddValueObjectSchema(schema);
                }
            }

            return dddModel;
        }
    }
}
