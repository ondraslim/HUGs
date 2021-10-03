using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Extensions;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.IntegrationTests")]
namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal class SchemaValidator
    {
        private readonly DddDiagnosticsReporter diagnosticsReporter;

        public SchemaValidator(DddDiagnosticsReporter diagnosticsReporter)
        {
            this.diagnosticsReporter = diagnosticsReporter;
        }

        public bool ValidateSchema(DddObjectSchema schema, DddModel dddModel)
        {
            // TODO: refactor not to use bool
            return schema.Kind switch
            {
                DddObjectKind.Enumeration => ValidateEnumeration(schema, dddModel),
                DddObjectKind.ValueObject or DddObjectKind.Entity or DddObjectKind.Aggregate => ValidateSchemaCommon(schema, dddModel),
                _ => throw new ArgumentOutOfRangeException(nameof(schema.Kind))
            };
        }

        private bool ValidateEnumeration(DddObjectSchema schema, DddModel dddModel)
        {
            return ValidateSchemaCommon(schema, dddModel) & ValidateValues(schema);
        }

        private bool ValidateSchemaCommon(DddObjectSchema schema, DddModel dddModel)
        {
            return ValidateSchemaName(schema) & ValidateProperties(schema, dddModel);
        }

        private bool ValidateSchemaName(DddObjectSchema schema)
        {
            if (!SyntaxFacts.IsValidIdentifier(schema.Name))
            {
                diagnosticsReporter.ReportSchemaInvalidValue(schema.Name, nameof(DddObjectSchema.Name), schema.Name);
                return false;
            }

            return true;
        }

        private bool ValidateProperties(DddObjectSchema schema, DddModel dddModel)
        {
            schema.Properties ??= new DddObjectProperty[] { };
            var isValid = ValidatePropertyNameUniqueness(schema.Properties);

            foreach (var property in schema.Properties)
            {
                if (!SyntaxFacts.IsValidIdentifier(property.Name) )
                {
                    diagnosticsReporter.ReportSchemaInvalidValue(
                        schema.Name, $"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Name)}", property.Name);
                    isValid = false;
                }

                if (!HasPropertyValidType(property, dddModel))
                {
                    diagnosticsReporter.ReportSchemaInvalidValue(
                        schema.Name, $"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Type)}", property.Type);
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool ValidatePropertyNameUniqueness(DddObjectProperty[] schemaProperties)
        {
            var names = schemaProperties.Select(e => e.Name);
            var duplicates = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(d => d.Key).ToArray();
            foreach (var duplicate in duplicates)
            {
                diagnosticsReporter.ReportDuplicatedDddObjectNames(duplicate);
            }

            return !duplicates.Any();
        }

        private bool ValidateValues(DddObjectSchema schema)
        {
            var isValid = true;
            schema.Values ??= new DddObjectValue[] { };

            foreach (var value in schema.Values)
            {
                if (!SyntaxFacts.IsValidIdentifier(value.Name))
                {
                    diagnosticsReporter.ReportSchemaInvalidValue(
                        schema.Name, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Name)}", value.Name);
                    isValid = false;
                }

                if (!ValidateValue(schema, value))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private static bool HasPropertyValidType(DddObjectProperty property, DddModel dddModel)
        {
            return property.IsWhitelistedType() || property.IsDddModelType(dddModel);
        }

        private bool ValidateValue(DddObjectSchema schema, DddObjectValue value)
        {
            var isValid = true;
            value.Properties ??= new Dictionary<string, string>();

            foreach (var property in value.Properties)
            {
                var propertyName = property.Key;

                var schemaProperty = schema.Properties.FirstOrDefault(p => p.Name == propertyName);
                if (schemaProperty == default || !SyntaxFacts.IsValidIdentifier(propertyName))
                {
                    diagnosticsReporter.ReportSchemaInvalidValue(schema.Name, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Properties)}_Key", propertyName);
                    isValid = false;
                }
            }

            return isValid;
        }

    }
}