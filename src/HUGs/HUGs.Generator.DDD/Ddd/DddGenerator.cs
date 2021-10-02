using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Loaders;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis;
using System;

namespace HUGs.Generator.DDD.Ddd
{
    public static class DddGenerator
    {
        private static DddDiagnosticsReporter _diagnosticsReporter;

        public static void Initialize(GeneratorInitializationContext context)
        {
            // not needed
        }

        public static void Execute(GeneratorExecutionContext context)
        {
            _diagnosticsReporter = new DddDiagnosticsReporter(context);

            try
            {
                var configuration = ConfigurationLoader.LoadConfiguration(context);
                var dddModel = DddModelLoader.LoadDddModel(context);

                GenerateDddModelSource(context, configuration, dddModel);
            }
            catch (LoadException e)
            {
                _diagnosticsReporter.ReportDiagnostic(e);
            }
        }

        private static void GenerateDddModelSource(
            GeneratorExecutionContext context,
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            foreach (var objectSchema in dddModel.Schemas)
            {
                AddDddObjectSchemaSource(context, objectSchema, configuration);
            }
        }

        private static void AddDddObjectSchemaSource(
            GeneratorExecutionContext context,
            DddObjectSchema objectSchema,
            DddGeneratorConfiguration configuration)
        {
            var sourceCode = GenerateDddObjectCode(objectSchema, configuration);
            context.AddSource(objectSchema.SourceCodeFileName, sourceCode);
        }

        private static string GenerateDddObjectCode(DddObjectSchema objectSchema, DddGeneratorConfiguration configuration)
            => objectSchema.Kind switch
            {
                DddObjectKind.ValueObject => ValueObjectGenerator.GenerateValueObjectCode(objectSchema, configuration),
                DddObjectKind.Entity => IdentifiableGenerator.GenerateEntityCode(objectSchema, configuration),
                DddObjectKind.Aggregate => IdentifiableGenerator.GenerateAggregateCode(objectSchema, configuration),
                DddObjectKind.Enumeration => EnumerationGenerator.GenerateEnumerationCode(objectSchema, configuration),
                _ => throw new ArgumentOutOfRangeException($"Kind '{objectSchema.Kind}' is not supported.")
            };

    }
}