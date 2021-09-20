using HUGs.Generator.DDD.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HUGs.Generator.DDD.Ddd
{
    public static class DddGenerator
    {
        public static DddModel DddModel { get; private set; }

        public static void Initialize(GeneratorInitializationContext context)
        {
            // not needed
        }

        public static void Load(GeneratorExecutionContext context)
        {
            var dddSchemas = GetDddSchemaFiles(context);

            // TODO: throw excpetion if not correct; catch -> diagnostics
            var model = BuildDddModel(dddSchemas);
            
            // TODO: validate model

            DddModel = model;
        }

        public static void Execute(GeneratorExecutionContext context)
        {
            GenerateDddModelSource(context);
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

                var dddSchema = deserializer.Deserialize<DddObjectSchema>(schemaText);
                if (dddSchema is null) continue; // TODO: raise diagnostics exception

                dddModel.AddObjectSchema(dddSchema);
            }

            return dddModel;
        }

        private static void GenerateDddModelSource(GeneratorExecutionContext context)
        {
            foreach (var objectSchema in DddModel.ObjectSchemas)
            {
                AddDddObjectSchemaSource(context, objectSchema);
            }
        }

        private static void AddDddObjectSchemaSource(GeneratorExecutionContext context, DddObjectSchema objectSchema)
        {
            var sourceCode = DddGenerator.GenerateDddObjectCode(objectSchema);
            context.AddSource($"{objectSchema.Name}{objectSchema.Kind}", sourceCode);
        }

        private static string GenerateDddObjectCode(DddObjectSchema objectSchema)
            => objectSchema.Kind switch
            {
                DddObjectKind.ValueObject => ValueObjectGenerator.GenerateValueObjectCode(objectSchema),
                DddObjectKind.Entity => IdentifiableGenerator.GenerateEntityCode(objectSchema),
                DddObjectKind.Aggregate => IdentifiableGenerator.GenerateAggregateCode(objectSchema),
                DddObjectKind.Enumeration => EnumerationGenerator.GenerateEnumerationCode(objectSchema),
                _ => throw new ArgumentOutOfRangeException($"Kind '{objectSchema.Kind}' is not supported.")
            };
    }
}