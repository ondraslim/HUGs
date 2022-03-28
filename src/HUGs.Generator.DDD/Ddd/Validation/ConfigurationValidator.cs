using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;


namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal static class ConfigurationValidator
    {
        private static readonly ICollection<Diagnostic> ValidationErrors = new List<Diagnostic>();

        /// <summary>
        /// Validates provided configuration object -  Assures valid namespaces and usings are provided.
        /// Otherwise, exception is thrown.
        /// </summary>
        public static void ValidateConfiguration(DddGeneratorConfiguration configuration)
        {
            ValidationErrors.Clear();

            ValidateTargetNamespaces(configuration.TargetNamespaces);
            ValidateAdditionalUsings(configuration.AdditionalUsings);

            if (ValidationErrors.Any())
            {
                throw new DddConfigurationValidationException(ValidationErrors);
            }
        }

        private static void ValidateTargetNamespaces(DddObjectTargetNamespaces targetNamespaces)
        {
            targetNamespaces ??= new DddObjectTargetNamespaces();

            ValidateTargetNamespace(targetNamespaces.Aggregate, nameof(DddObjectTargetNamespaces.Aggregate));
            ValidateTargetNamespace(targetNamespaces.Entity, nameof(DddObjectTargetNamespaces.Entity));
            ValidateTargetNamespace(targetNamespaces.Enumeration, nameof(DddObjectTargetNamespaces.Enumeration));
            ValidateTargetNamespace(targetNamespaces.ValueObject, nameof(DddObjectTargetNamespaces.ValueObject));
        }

        private static void ValidateTargetNamespace(string @namespace, string namespaceTarget)
        {
            if (!@namespace.Split('.').All(SyntaxFacts.IsValidIdentifier))
            {
                var diagnostic = DddDiagnostics.GetConfigurationInvalidValueDiagnostic(
                    namespaceTarget, $"{nameof(DddGeneratorConfiguration.TargetNamespaces)}_{namespaceTarget}");
                ValidationErrors.Add(diagnostic);
            }
        }

        private static void ValidateAdditionalUsings(string[] additionalUsings)
        {
            additionalUsings ??= new string[] { };

            foreach (var additionalUsing in additionalUsings)
            {
                if (!additionalUsing.Split('.').All(SyntaxFacts.IsValidIdentifier))
                {
                    var diagnostic = DddDiagnostics.GetConfigurationInvalidValueDiagnostic(additionalUsing, nameof(DddGeneratorConfiguration.AdditionalUsings));
                    ValidationErrors.Add(diagnostic);
                }
            }
        }
    }
}