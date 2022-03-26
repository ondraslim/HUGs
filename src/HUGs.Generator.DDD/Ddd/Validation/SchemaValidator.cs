using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Extensions;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HUGs.Generator.DDD.IntegrationTests")]
namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal static class SchemaValidator
    {
        private static readonly ICollection<Diagnostic> ValidationErrors = new List<Diagnostic>();

        /// <summary>
        /// Validates schema. Assures syntax-valid schema name and property definitions. For Enumerations, also checks enum values.
        /// </summary>
        public static void ValidateSchema(DddObjectSchema schema, DddModel dddModel)
        {
            ValidationErrors.Clear();

            ValidateSchemaName(schema);
            ValidateProperties(schema, dddModel);

            if (schema.Kind is DddObjectKind.Enumeration)
            {
                ValidateValues(schema);
            }

            if (ValidationErrors.Any())
            {
                throw new DddSchemaValidationException(ValidationErrors);
            }
        }

        private static void ValidateSchemaName(DddObjectSchema schema)
        {
            if (!SyntaxFacts.IsValidIdentifier(schema.Name))
            {
                var diagnostic = DddDiagnostics.GetSchemaInvalidValueDiagnostic(schema.Name, nameof(DddObjectSchema.Name), schema.Name);
                ValidationErrors.Add(diagnostic);
            }
        }

        private static void ValidateProperties(DddObjectSchema schema, DddModel dddModel)
        {
            schema.Properties ??= new DddObjectProperty[] { };
            
            ValidatePropertyNameUniqueness(schema.Properties);

            foreach (var property in schema.Properties)
            {
                if (!SyntaxFacts.IsValidIdentifier(property.Name))
                {
                    var diagnostic = DddDiagnostics.GetSchemaInvalidValueDiagnostic(
                        property.Name, $"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Name)}", schema.Name);
                    ValidationErrors.Add(diagnostic);
                }

                if (!IsValidPropertyType(property, dddModel))
                {
                    var diagnostic = DddDiagnostics.GetSchemaInvalidValueDiagnostic(
                        property.Type,$"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Type)}", schema.Name);
                    ValidationErrors.Add(diagnostic);
                }
            }
        }

        private static void ValidatePropertyNameUniqueness(IEnumerable<DddObjectProperty> schemaProperties)
        {
            var names = schemaProperties.Select(e => e.Name);
            var duplicatedNames = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(d => d.Key).ToArray();
            foreach (var duplicatedName in duplicatedNames)
            {
                ValidationErrors.Add(DddDiagnostics.GetDuplicatedDddObjectNamesDiagnostic(duplicatedName));
            }
        }

        private static void ValidateValues(DddObjectSchema schema)
        {
            schema.Values ??= new DddObjectValue[] { };

            foreach (var value in schema.Values)
            {
                if (!SyntaxFacts.IsValidIdentifier(value.Name))
                {
                    var diagnostic = DddDiagnostics.GetSchemaInvalidValueDiagnostic(
                        value.Name, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Name)}", schema.Name);
                    ValidationErrors.Add(diagnostic);
                }

                ValidateValue(schema, value);
            }
        }

        private static bool IsValidPropertyType(DddObjectProperty property, DddModel dddModel)
        {
            return property.IsPrimitiveType() || property.IsKnownDddModelType(dddModel);
        }

        private static void ValidateValue(DddObjectSchema schema, DddObjectValue value)
        {
            value.Properties ??= new Dictionary<string, string>();

            foreach (var property in value.Properties)
            {
                var propertyName = property.Key;

                var schemaProperty = schema.Properties.FirstOrDefault(p => p.Name == propertyName);
                if (schemaProperty == default || !SyntaxFacts.IsValidIdentifier(propertyName))
                {
                    var diagnostic = DddDiagnostics.GetSchemaInvalidValueDiagnostic(propertyName, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Properties)}_Key", schema.Name);
                    ValidationErrors.Add(diagnostic);
                }
            }
        }

    }
}