using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal static class DddModelValidator
    {
        private static readonly ICollection<Diagnostic> ValidationErrors = new List<Diagnostic>();

        public static void ValidateModel(DddModel model)
        {
            ValidationErrors.Clear();

            ValidateIndividualSchemas(model);
            ValidateObjectNameUniqueness(model);

            if (ValidationErrors.Any())
            {
                throw new DddModelValidationException(ValidationErrors);
            }
        }

        private static void ValidateIndividualSchemas(DddModel model)
        {
            foreach (var schema in model.Schemas)
            {
                SchemaValidator.ValidateSchema(schema, model);
            }
        }

        private static void ValidateObjectNameUniqueness(DddModel model)
        {
            var names = model.Schemas.Select(e => e.Name);
            var duplicatedNames = names.GroupBy(n => n).Where(g => g.Count() > 1).Select(d => d.Key).ToArray();
            foreach (var duplicatedName in duplicatedNames)
            {
                ValidationErrors.Add(DddDiagnostics.GetDuplicatedDddObjectNamesDiagnostic(duplicatedName));
            }
        }
    }
}