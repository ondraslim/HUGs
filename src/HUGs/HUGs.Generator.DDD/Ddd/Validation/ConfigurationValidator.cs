using System.Linq;
using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;


namespace HUGs.Generator.DDD.Ddd.Validation
{
    internal class ConfigurationValidator
    {
        private readonly DiagnosticReporter diagnosticReporter;

        public ConfigurationValidator(DiagnosticReporter diagnosticReporter)
        {
            this.diagnosticReporter = diagnosticReporter;
        }

        public bool ValidateConfiguration(DddGeneratorConfiguration configuration)
        {
            return ValidateTargetNamespaces(configuration.TargetNamespaces) &
                   ValidateAdditionalUsings(configuration.AdditionalUsings);
        }

        private bool ValidateTargetNamespaces(DddObjectTargetNamespaces targetNamespaces)
        {
            targetNamespaces ??= new DddObjectTargetNamespaces();

            return ValidateTargetNamespace(targetNamespaces.Aggregate, nameof(DddGeneratorConfiguration.TargetNamespaces.Aggregate), DddObjectTargetNamespaces.DefaultAggregateNamespace) &
                   ValidateTargetNamespace(targetNamespaces.Entity, nameof(DddGeneratorConfiguration.TargetNamespaces.Entity), DddObjectTargetNamespaces.DefaultEntityNamespace) &
                   ValidateTargetNamespace(targetNamespaces.Enumeration, nameof(DddGeneratorConfiguration.TargetNamespaces.Enumeration), DddObjectTargetNamespaces.DefaultEnumerationNamespace) &
                   ValidateTargetNamespace(targetNamespaces.ValueObject, nameof(DddGeneratorConfiguration.TargetNamespaces.ValueObject), DddObjectTargetNamespaces.DefaultValueObjectNamespace);
        }

        private bool ValidateTargetNamespace(string @namespace, string namespaceTarget, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
            {
                @namespace = defaultValue;
            }

            if (!@namespace.Split('.').All(SyntaxFacts.IsValidIdentifier))
            {
                diagnosticReporter.ReportDiagnostic(DddDiagnostic.GetConfigurationInvalidValueDiagnostic(
                    namespaceTarget,
                    $"{nameof(DddGeneratorConfiguration.TargetNamespaces)}_{namespaceTarget}"));

                return false;
            }

            return true;
        }

        private bool ValidateAdditionalUsings(string[] additionalUsings)
        {
            additionalUsings ??= new string[] { };

            var isValid = true;
            foreach (var additionalUsing in additionalUsings)
            {
                if (!additionalUsing.Split('.').All(SyntaxFacts.IsValidIdentifier))
                {
                    diagnosticReporter.ReportDiagnostic(DddDiagnostic.GetConfigurationInvalidValueDiagnostic(additionalUsing, nameof(DddGeneratorConfiguration.AdditionalUsings)));
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}