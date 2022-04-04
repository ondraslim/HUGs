using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HUGs.Generator.DDD.Validators;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    /// <summary>
    /// Takes care of loading a DDD model based on a .dddschema file.
    /// </summary>
    public class DomainModelLoader
    {
        private static DddDiagnosticsReporter _diagnosticsReporter;

        private static void InitializeDependencies(GeneratorExecutionContext context)
        {
            _diagnosticsReporter = new DddDiagnosticsReporter(context);
        }

        /// <summary>
        /// Loads a domain model based on all the schema files available in the GeneratorExecutionContext
        /// </summary>
        public static DomainModel LoadDomainModel(GeneratorExecutionContext context)
        {
            InitializeDependencies(context);

            var schemas = GetDddSchemaFiles(context);
            return LoadDomainModel(schemas);
        }

        private static IEnumerable<AdditionalText> GetDddSchemaFiles(GeneratorExecutionContext context)
        {
            return context
                .AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddschema", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private static DomainModel LoadDomainModel(IEnumerable<AdditionalText> dddSchemas)
        {
            var model = BuildDomainModel(dddSchemas);
            DomainModelValidator.ValidateDomainModel(model);
            ResolvePropertyTypes(model);
            return model;
        }

        /// <summary>
        /// Adds resolved DDD types to each property of each schema within the model.
        /// </summary>
        private static void ResolvePropertyTypes(DomainModel model)
        {
            foreach (var schema in model.Schemas)
            {
                foreach (var prop in schema.Properties)
                {
                    prop.ResolvedType = DddType.Parse(prop.Type, model);
                }
            }
        }

        /// <summary>
        /// Transforms the .dddschema content to C# object representation.
        /// </summary>
        private static DomainModel BuildDomainModel(IEnumerable<AdditionalText> schemaFiles)
        {
            var dddModel = new DomainModel();
            foreach (var schemaFile in schemaFiles)
            {
                var schemaText = schemaFile.GetText()?.ToString();

                if (string.IsNullOrWhiteSpace(schemaText))
                {
                    _diagnosticsReporter.ReportEmptyAdditionalFile(schemaFile.Path);;
                    continue;
                }

                try
                {
                    var dddSchema = LoaderCommon.Deserialize<DddObjectSchema>(schemaText);
                    dddModel.AddObjectSchema(dddSchema);
                }
                catch (Exception e)
                {
                    throw new AdditionalFileParseException(schemaFile.Path, $"Error occurred while parsing file: {schemaFile.Path}", e);
                }
            }

            return dddModel;
        }
    }
}