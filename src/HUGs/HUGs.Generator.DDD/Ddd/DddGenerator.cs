using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
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
        private static DiagnosticReporter _diagnosticReporter;

        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public static DddModel DddModel { get; private set; }
        public static DddGeneratorConfiguration GeneratorConfiguration { get; private set; }

        public static bool LoadFinishedSuccessfully { get; private set; }

        public static void Initialize(GeneratorInitializationContext context)
        {
            GeneratorConfiguration = new DddGeneratorConfiguration();
            LoadFinishedSuccessfully = true;
        }

        public static void Execute(GeneratorExecutionContext context)
        {
            Load(context);
            if (LoadFinishedSuccessfully)
            {
                GenerateDddModelSource(context);
            }
        }

        private static void Load(GeneratorExecutionContext context)
        {
            _diagnosticReporter = new DiagnosticReporter(context);

            var configurationFile = GetDddConfiguration(context);
            var dddSchemas = GetDddSchemaFiles(context);

            if (!dddSchemas.Any())
            {
                LoadFinishedSuccessfully = false;
                return;
            }

            LoadConfiguration(configurationFile);
            LoadDddModel(dddSchemas);
        }

        private static void LoadConfiguration(AdditionalText configurationFile)
        {
            try
            {
                var configuration = BuildConfiguration(configurationFile);
                // TODO: validate configuration
                if (configuration is not null)
                {
                    GeneratorConfiguration = configuration;
                }
            }
            catch (AdditionalFileParseException e)
            {
                _diagnosticReporter.ReportDiagnostic(Diagnostic.Create(
                    DddDiagnostics.AdditionalFileParseError, 
                    Location.None, 
                    DiagnosticSeverity.Error, 
                    e.FilePath,
                     e.InnerException?.Message ?? e.Message));
            }
        }

        private static void LoadDddModel(IEnumerable<AdditionalText> dddSchemas)
        {
            try
            {
                var model = BuildDddModel(dddSchemas);
                // TODO: validate model
                DddModel = model;
            }
            catch (AdditionalFileParseException e)
            {
                _diagnosticReporter.ReportDiagnostic(Diagnostic.Create(
                    DddDiagnostics.AdditionalFileParseError, 
                    Location.None, 
                    DiagnosticSeverity.Error,
                    e.FilePath,
                    e.InnerException?.Message ?? e.Message));
            }
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
                _diagnosticReporter.ReportDiagnostic(Diagnostic.Create(DddDiagnostics.ConfigurationConflictError, Location.None, foundFileNames));
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

        private static DddGeneratorConfiguration BuildConfiguration(AdditionalText configurationFile)
        {
            var configurationText = configurationFile?.GetText()?.ToString();

            if (string.IsNullOrWhiteSpace(configurationText)) return null;

            try
            {
                var configuration = Deserializer.Deserialize<DddGeneratorConfiguration>(configurationText);
                return configuration;
            }
            catch (Exception e)
            {
                LoadFinishedSuccessfully = false;
                throw new AdditionalFileParseException($"Error occurred while parsing file: {configurationFile.Path}", configurationFile.Path, e);
            }
        }

        private static DddModel BuildDddModel(IEnumerable<AdditionalText> schemaFiles)
        {
            var dddModel = new DddModel();
            foreach (var schemaFile in schemaFiles)
            {
                var schemaText = schemaFile.GetText()?.ToString();

                if (string.IsNullOrWhiteSpace(schemaText))
                {
                    _diagnosticReporter.ReportDiagnostic(Diagnostic.Create(
                        DddDiagnostics.EmptyAdditionalFileWarning,
                        Location.None, 
                        DiagnosticSeverity.Warning, 
                        schemaFile.Path));
                    continue;
                };

                try
                {
                    var dddSchema = Deserializer.Deserialize<DddObjectSchema>(schemaText);
                    dddModel.AddObjectSchema(dddSchema);
                }
                catch (Exception e)
                {
                    LoadFinishedSuccessfully = false;
                    throw new AdditionalFileParseException($"Error occurred while parsing file: {schemaFile.Path}", schemaFile.Path, e);
                }
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