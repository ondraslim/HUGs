using System.Collections.Generic;
using System.Linq;
using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Models;

namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal class ModelValidator
    {
        private readonly DiagnosticReporter diagnosticReporter;
        private readonly SchemaValidator schemaValidator;

        public ModelValidator(DiagnosticReporter diagnosticReporter, SchemaValidator schemaValidator)
        {
            this.diagnosticReporter = diagnosticReporter;
            this.schemaValidator = schemaValidator;
        }

        public bool ValidateModel(DddModel model)
        {
            ValidateIndividualSchemas(model);

            return ModelHasUniqueObjectNames(model);
        }

        private void ValidateIndividualSchemas(DddModel model)
        {
            foreach (var schema in model.Schemas)
            {
                if (!schemaValidator.ValidateSchema(schema))
                {
                    throw new DddSchemaValidationException(schema.Name);
                }
            }
        }

        private bool ModelHasUniqueObjectNames(DddModel model)
        {
            return NamesAreUnique(model.Entities.Select(e => e.Name)) &
                   NamesAreUnique(model.Aggregates.Select(e => e.Name)) &
                   NamesAreUnique(model.ValueObjects.Select(e => e.Name)) &
                   NamesAreUnique(model.Enumerations.Select(e => e.Name));
        }

        public bool NamesAreUnique(IEnumerable<string> names)
        {
            var duplicates = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(d => d.Key).ToArray();
            foreach (var duplicate in duplicates)
            {
                diagnosticReporter.ReportDiagnostic(DddDiagnostic.GetDuplicatedDddObjectNamesDiagnostic(duplicate));
            }

            return !duplicates.Any();
        }
    }
}