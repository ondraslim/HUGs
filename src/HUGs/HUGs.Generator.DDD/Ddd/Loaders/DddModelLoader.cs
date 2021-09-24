﻿using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Ddd.Diagnostics;
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

        public static DddModel LoadDddModel(GeneratorExecutionContext context)
        {
            _diagnosticReporter = new DiagnosticReporter(context);

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
            // TODO: validate model
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
                        DddDiagnostics.EmptyAdditionalFileWarning,
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