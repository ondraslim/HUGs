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
            catch (GeneratorLoadException e)
            {
                _diagnosticsReporter.ReportDiagnostic(e);
            }
        }

        private static void GenerateDddModelSource(
            GeneratorExecutionContext context,
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            foreach (var schema in dddModel.Schemas)
            {
                AddDddObjectSchemaSource(context, schema, configuration);

                if (schema.Kind != DddObjectKind.Enumeration)
                {
                    AddDbEntitySource(context, schema, configuration, dddModel);
                }
            }
        }

        private static void AddDddObjectSchemaSource(
            GeneratorExecutionContext context,
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration)
        {
            var dddObjectSourceCode = GenerateDddObjectCode(schema, configuration);
            context.AddSource(schema.SourceCodeFileName, dddObjectSourceCode);
        }

        private static void AddDbEntitySource(
            GeneratorExecutionContext context,
            DddObjectSchema schema, 
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            var dbEntitySourceCode = DbEntityGenerator.GenerateDbEntity(schema, configuration, dddModel);
            context.AddSource(schema.DbEntityFileName, dbEntitySourceCode);
        }

        private static string GenerateDddObjectCode(DddObjectSchema schema, DddGeneratorConfiguration configuration)
            => schema.Kind switch
            {
                DddObjectKind.ValueObject => ValueObjectGenerator.GenerateValueObjectCode(schema, configuration),
                DddObjectKind.Entity => EntityGenerator.GenerateEntityCode(schema, configuration),
                DddObjectKind.Aggregate => AggregateGenerator.GenerateAggregateCode(schema, configuration),
                DddObjectKind.Enumeration => EnumerationGenerator.GenerateEnumerationCode(schema, configuration),
                _ => throw new ArgumentOutOfRangeException($"Kind '{schema.Kind}' is not supported.")
            };

    }
}