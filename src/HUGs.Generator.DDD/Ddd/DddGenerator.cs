﻿using HUGs.Generator.Common.Builders;
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

                GenerateNamespaceDeclarationFile(context, configuration);
                GenerateDddModelSourceCode(context, configuration, dddModel);
            }
            catch (GeneratorLoadException e)
            {
                _diagnosticsReporter.ReportDiagnostic(e);
            }
        }

        /// <summary>
        /// Generates an empty file with commonly used generated namespaces
        /// </summary>
        private static void GenerateNamespaceDeclarationFile(GeneratorExecutionContext context, DddGeneratorConfiguration configuration)
        {
            var namespaceDeclarations = RoslynSyntaxNamespacesFillerBuilder.Create();

            foreach (DddObjectKind kind in Enum.GetValues(typeof(DddObjectKind)))
            {
                namespaceDeclarations.AddNamespaces(configuration.GetTargetNamespaceForKind(kind));
            }

            namespaceDeclarations.AddNamespaces(configuration.TargetNamespaces.DbEntity);

            var namespacesDeclarationsSourceCode = namespaceDeclarations.Build();
            context.AddSource("DddModelNamespaces", namespacesDeclarationsSourceCode);
        }

        /// <summary>
        /// Generates source code files for schemas
        /// </summary>
        private static void GenerateDddModelSourceCode(
            GeneratorExecutionContext context,
            DddGeneratorConfiguration configuration,
            DddModel dddModel)
        {
            foreach (var schema in dddModel.Schemas)
            {
                AddDddObjectSchemaSource(context, schema, configuration);

                if (schema.Kind is not DddObjectKind.Enumeration)
                {
                    AddDbEntitySource(context, schema, configuration);
                    //AddMapperSource(context, schema, configuration);
                }
            }
        }

        private static void AddMapperSource(
            GeneratorExecutionContext context, 
            DddObjectSchema schema, 
            DddGeneratorConfiguration configuration)
        {
            var mapperSourceCode = MapperGenerator.GenerateMapperCode(schema, configuration);
            context.AddSource(schema.MapperClassName, mapperSourceCode);
        }

        private static void AddDddObjectSchemaSource(
            GeneratorExecutionContext context,
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration)
        {
            var dddObjectSourceCode = GenerateDddObjectCode(schema, configuration);
            context.AddSource(schema.DddObjectClassName, dddObjectSourceCode);
        }

        private static void AddDbEntitySource(
            GeneratorExecutionContext context,
            DddObjectSchema schema, 
            DddGeneratorConfiguration configuration)
        {
            var dbEntitySourceCode = DbEntityGenerator.GenerateDbEntity(schema, configuration);
            context.AddSource(schema.DbEntityClassName, dbEntitySourceCode);
        }

        private static string GenerateDddObjectCode(
            DddObjectSchema schema,
            DddGeneratorConfiguration configuration)
            => schema.Kind switch
            {
                DddObjectKind.ValueObject => ValueObjectGenerator.GenerateValueObjectCode(schema, configuration),
                DddObjectKind.Entity => EntityGenerator.GenerateEntityCode(schema, configuration),
                DddObjectKind.Aggregate => AggregateGenerator.GenerateAggregateCode(schema, configuration),
                DddObjectKind.Enumeration => EnumerationGenerator.GenerateEnumerationCode(schema, configuration),
                _ => throw new NotSupportedException($"Generation for kind '{schema.Kind}' is not supported.")
            };

    }
}