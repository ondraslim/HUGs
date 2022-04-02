using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal static class DomainModelValidator
    {
        private static readonly ICollection<Diagnostic> ValidationErrors = new List<Diagnostic>();

        /// <summary>
        /// Validates domain model by validating its separate DDD object schemas and validates uniqueness of names of the object schemas (unique names are required!).
        /// </summary>
        public static void ValidateDomainModel(DomainModel model)
        {
            ValidationErrors.Clear();

            ValidateIndividualSchemas(model);
            ValidateObjectNameUniqueness(model);

            if (ValidationErrors.Any())
            {
                throw new DomainModelValidationException(ValidationErrors);
            }
        }

        private static void ValidateIndividualSchemas(DomainModel model)
        {
            foreach (var schema in model.Schemas)
            {
                SchemaValidator.ValidateSchema(schema, model);
            }
        }

        private static void ValidateObjectNameUniqueness(DomainModel model)
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