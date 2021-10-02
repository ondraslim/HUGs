using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal class ModelValidator
    {
        private readonly DddDiagnosticsReporter diagnosticReporter;
        private readonly SchemaValidator schemaValidator;

        public ModelValidator(DddDiagnosticsReporter diagnosticReporter, SchemaValidator schemaValidator)
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
            var names = model.Schemas.Select(e => e.Name);
            var duplicates = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(d => d.Key).ToArray();
            foreach (var duplicate in duplicates)
            {
                diagnosticReporter.ReportDuplicatedDddObjectNames(duplicate);
            }

            return !duplicates.Any();
        }
    }
}