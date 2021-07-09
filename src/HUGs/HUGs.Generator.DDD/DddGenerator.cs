using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private static IEnumerable<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context
                .AdditionalFiles
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

        private static void GenerateDddModelSource(GeneratorExecutionContext context, DddModel dddModel)
        {
            foreach (var valueObject in dddModel.ValueObjects)
            {
                AddValueObjectSource(context, valueObject);
            }
        }

        private static void AddValueObjectSource(GeneratorExecutionContext context, DddObjectSchema valueObject)
        {
            var valueObjectClass = ValueObjectGenerator.GenerateValueObjectCode(valueObject);
            context.AddSource($"{valueObject.Name}ValueObject", valueObjectClass);
        }
    }
}
