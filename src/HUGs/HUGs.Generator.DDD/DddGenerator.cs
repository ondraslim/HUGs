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
                if (schema is null) continue; // TODO: raise diagnostics exception

                if (schema.Kind.Equals("ValueObject", StringComparison.InvariantCultureIgnoreCase))
                {
                    dddModel.AddValueObjectSchema(schema);
                }
                else if (schema.Kind.Equals("Entity", StringComparison.InvariantCultureIgnoreCase))
                {
                    dddModel.AddEntitySchema(schema);
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

            foreach (var entity in dddModel.Entities)
            {
                AddEntitySource(context, entity);
            }
        }

        private static void AddValueObjectSource(GeneratorExecutionContext context, DddObjectSchema valueObject)
        {
            var valueObjectClass = ValueObjectGenerator.GenerateValueObjectCode(valueObject);
            context.AddSource($"{valueObject.Name}ValueObject", valueObjectClass);
        }

        private static void AddEntitySource(GeneratorExecutionContext context, DddObjectSchema entity)
        {
            var entityClass = EntityGenerator.GenerateEntityCode(entity);
            context.AddSource($"{entity.Name}Entity", entityClass);
        }
    }
}
