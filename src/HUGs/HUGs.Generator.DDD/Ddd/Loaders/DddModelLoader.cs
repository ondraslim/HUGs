using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Validation;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    public class DddModelLoader
    {
        private static DiagnosticReporter _diagnosticReporter;
        private static SchemaValidator _schemaValidator;

        private static void InitializeDependencies(GeneratorExecutionContext context)
        {
            _diagnosticReporter = new DiagnosticReporter(context);
            _schemaValidator = new SchemaValidator(_diagnosticReporter);
        }

        public static DddModel LoadDddModel(GeneratorExecutionContext context)
        {
            InitializeDependencies(context);

            var schemas = GetDddSchemaFiles(context);
            return LoadDddModel(schemas);
        }

        private static IEnumerable<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context
                .AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddschema", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static DddModel LoadDddModel(IEnumerable<AdditionalText> dddSchemas)
        {
            var model = BuildDddModel(dddSchemas);

            // TODO: move to ModelValidator
            foreach (var schema in model.Schemas)
            {
                if (!_schemaValidator.ValidateSchema(schema))
                {
                    throw new DddSchemaValidationException(schema.Name);
                }
            }
            return model;
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
                        DddDiagnostic.EmptyAdditionalFileWarning,
                        Location.None,
                        DiagnosticSeverity.Warning,
                        schemaFile.Path));
                    continue;
                }

                try
                {
                    var dddSchema = LoaderCommon.Deserialize<DddObjectSchema>(schemaText);
                    dddModel.AddObjectSchema(dddSchema);
                }
                catch (Exception e)
                {
                    throw new AdditionalFileParseException($"Error occurred while parsing file: {schemaFile.Path}", schemaFile.Path, e);
                }
            }

            return dddModel;
        }
    }
}