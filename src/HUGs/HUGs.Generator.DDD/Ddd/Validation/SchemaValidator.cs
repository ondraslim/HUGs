using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal class SchemaValidator
    {
        private readonly DiagnosticReporter diagnosticReporter;

        public SchemaValidator(DiagnosticReporter diagnosticReporter)
        {
            this.diagnosticReporter = diagnosticReporter;
        }

        public bool ValidateSchema(DddObjectSchema schema)
        {
            return schema.Kind switch
            {
                DddObjectKind.ValueObject => ValidateValueObject(schema),
                DddObjectKind.Entity => ValidateEntity(schema),
                DddObjectKind.Aggregate => ValidateAggregate(schema),
                DddObjectKind.Enumeration => ValidateEnumeration(schema),
                _ => throw new ArgumentOutOfRangeException(nameof(schema.Kind))
            };
        }

        private bool ValidateValueObject(DddObjectSchema schema)
        {
            return ValidateSchemaName(schema) & ValidateProperties(schema);
        }

        private bool ValidateEntity(DddObjectSchema schema)
        {
            return ValidateSchemaName(schema) & ValidateProperties(schema);
        }

        private bool ValidateAggregate(DddObjectSchema schema)
        {
            return ValidateSchemaName(schema) & ValidateProperties(schema);
        }

        private bool ValidateEnumeration(DddObjectSchema schema)
        {
            return ValidateSchemaName(schema) & ValidateProperties(schema) & ValidateValues(schema);
        }

        private bool ValidateSchemaName(DddObjectSchema schema)
        {
            if (!SyntaxFacts.IsValidIdentifier(schema.Name))
            {
                diagnosticReporter.ReportDiagnostic(DddDiagnostics.GetInvalidSchemaDiagnostic(schema.Name, nameof(DddObjectSchema.Name), schema.Name));
                return false;
            }

            return true;
        }

        private bool ValidateProperties(DddObjectSchema schema)
        {
            var isValid = true;
            schema.Properties ??= new DddObjectProperty[] { };

            foreach (var property in schema.Properties)
            {
                if (!SyntaxFacts.IsValidIdentifier(property.Name))
                {
                    diagnosticReporter.ReportDiagnostic(DddDiagnostics.GetInvalidSchemaDiagnostic(
                        schema.Name, $"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Name)}", property.Name));
                    isValid = false;
                }
                if (!IsValidType(property.TypeWithoutArray))
                {
                    diagnosticReporter.ReportDiagnostic(DddDiagnostics.GetInvalidSchemaDiagnostic(
                        schema.Name, $"{nameof(DddObjectProperty)}_{nameof(DddObjectProperty.Type)}", property.Type));
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool ValidateValues(DddObjectSchema schema)
        {
            var isValid = true;
            schema.Values ??= new DddObjectValue[] { };

            foreach (var value in schema.Values)
            {
                if (!SyntaxFacts.IsValidIdentifier(value.Name))
                {
                    diagnosticReporter.ReportDiagnostic(DddDiagnostics.GetInvalidSchemaDiagnostic(
                        schema.Name, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Name)}", value.Name));
                    isValid = false;
                }

                if (!ValidateValue(schema, value))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private static bool IsValidType(string type)
        {
            try
            {
                var typeSyntax = SyntaxFactory.ParseTypeName(type);
                return SyntaxFacts.IsPredefinedType(typeSyntax.Kind()) || SyntaxFacts.IsValidIdentifier(type);
            }
            catch (Exception)
            {
                return false;
            }
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
                    diagnosticReporter.ReportDiagnostic(DddDiagnostics.GetInvalidSchemaDiagnostic(
                        schema.Name, $"{nameof(DddObjectValue)}_{nameof(DddObjectValue.Properties)}_Key", propertyName));
                    isValid = false;
                }
            }

            return isValid;
        }
        
    }
}