using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Common.Configuration;
using HUGs.Generator.DDD.Ddd.Diagnostics;
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
        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public static DddModel DddModel { get; private set; }
        public static DddGeneratorConfiguration GeneratorConfiguration { get; private set; }

        public static void Initialize(GeneratorInitializationContext context)
        {
            GeneratorConfiguration = new DddGeneratorConfiguration();
        }

        public static void Load(GeneratorExecutionContext context)
        {
            var configurationFile = GetDddConfiguration(context);
            var dddSchemas = GetDddSchemaFiles(context);

            if (!dddSchemas.Any())
            {
                return;
            }

            var configuration = BuildConfiguration(configurationFile);
            // TODO: validate configuration
            if (configuration is not null)
            {
                GeneratorConfiguration = configuration;
            }

            // TODO: throw exception if not correct; catch -> diagnostics
            var model = BuildDddModel(dddSchemas);
            // TODO: validate model
            DddModel = model;
        }

        private static DddGeneratorConfiguration BuildConfiguration(AdditionalText configurationFile)
        {
            var configurationText = configurationFile?.GetText()?.ToString();

            if (string.IsNullOrWhiteSpace(configurationText)) return null;

            var configuration = Deserializer.Deserialize<DddGeneratorConfiguration>(configurationText);
            if (configuration is null) return null; // TODO: raise diagnostics exception

            return configuration;
        }

        public static void Execute(GeneratorExecutionContext context)
        {
            GenerateDddModelSource(context);
        }

        private static AdditionalText GetDddConfiguration(GeneratorExecutionContext context)
        {
            var configurations = context
                .AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddconfig", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (configurations.Count > 1)
            {
                var foundFileNames = string.Join(", ", configurations.Select(c => $"'{c.Path}'"));
                context.ReportDiagnostic(Diagnostic.Create(DddDiagnostics.ConfigurationDiagnostics, Location.None, foundFileNames));
                return null;
            }

            return configurations.FirstOrDefault();
        }

        private static IList<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context
                .AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddschema", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static DddModel BuildDddModel(IEnumerable<AdditionalText> schemaFiles)
        {
            var dddModel = new DddModel();
            foreach (var schemaFile in schemaFiles)
            {
                var schemaText = schemaFile.GetText()?.ToString();

                if (string.IsNullOrWhiteSpace(schemaText)) continue;

                var dddSchema = Deserializer.Deserialize<DddObjectSchema>(schemaText);
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
            var sourceCode = GenerateDddObjectCode(objectSchema);
            context.AddSource($"{objectSchema.Name}{objectSchema.Kind}", sourceCode);
        }

        private static string GenerateDddObjectCode(DddObjectSchema objectSchema)
            => objectSchema.Kind switch
            {
                DddObjectKind.ValueObject => ValueObjectGenerator.GenerateValueObjectCode(objectSchema, GeneratorConfiguration),
                DddObjectKind.Entity => IdentifiableGenerator.GenerateEntityCode(objectSchema, GeneratorConfiguration),
                DddObjectKind.Aggregate => IdentifiableGenerator.GenerateAggregateCode(objectSchema, GeneratorConfiguration),
                DddObjectKind.Enumeration => EnumerationGenerator.GenerateEnumerationCode(objectSchema, GeneratorConfiguration),
                _ => throw new ArgumentOutOfRangeException($"Kind '{objectSchema.Kind}' is not supported.")
            };
    }
}