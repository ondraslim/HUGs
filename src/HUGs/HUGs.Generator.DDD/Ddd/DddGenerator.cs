using HUGs.Generator.Common.Diagnostics;
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
        private static DiagnosticReporter _diagnosticReporter;

        public static DddModel DddModel { get; private set; }
        public static DddGeneratorConfiguration GeneratorConfiguration { get; private set; }


        public static void Initialize(GeneratorInitializationContext context)
        {
            GeneratorConfiguration = new DddGeneratorConfiguration();
        }

        public static void Execute(GeneratorExecutionContext context)
        {
            _diagnosticReporter = new DiagnosticReporter(context);

            try
            {
                if (TryLoad(context))
                {
                    GenerateDddModelSource(context);
                }
            }
            catch (DddLoadException e)
            {
                _diagnosticReporter.ReportDiagnostic(DddDiagnostics.ExceptionToDiagnosticConverter(e));
            }
        }

        private static bool TryLoad(GeneratorExecutionContext context)
        {
            var configuration = ConfigurationLoader.LoadConfiguration(context);
            if (configuration is not null)
            {
                GeneratorConfiguration = configuration;
            }

            DddModel = DddModelLoader.LoadDddModel(context);

            return true;
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