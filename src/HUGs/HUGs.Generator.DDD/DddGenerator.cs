using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hugs.Generator.DDD
{
    [Generator]
    public class DddGenerator : ISourceGenerator
    {
        private IDeserializer deserializer;

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached) Debugger.Launch();
#endif

            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var dddSchemas = GetDddSchemaFiles(context);
            var dddModel = BuildDddModel(dddSchemas);

            //var sb = new StringBuilder(@"");
            // inject the created source into the users compilation
            //context.AddSource("HelloWorldGenerated", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static IEnumerable<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddschema", StringComparison.OrdinalIgnoreCase));
        }

        private DddModel BuildDddModel(IEnumerable<AdditionalText> schemaFiles)
        {
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
