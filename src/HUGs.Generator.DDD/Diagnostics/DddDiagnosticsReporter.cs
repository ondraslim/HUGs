using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Exceptions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public class DddDiagnosticsReporter : DiagnosticsReporter
    {
        public DddDiagnosticsReporter(GeneratorExecutionContext generatorExecutionContext) 
            : base(generatorExecutionContext)
        {
        }

        /// <summary>
        /// Reports a diagnostic based on an exception.
        /// </summary>
        public void ReportDiagnostic(GeneratorLoadException e)
        {
            base.ReportDiagnostic(e);

            switch (e)
            {
                case DddMultipleConfigurationsFoundException ex:
                    ReportConfigurationMultiple(ex);
                    break;
                case DddConfigurationValidationException ex:
                    ReportConfigurationInvalid(ex);
                    break;
                case DddSchemaValidationException ex:
                    ReportSchemaInvalid(ex);
                    break;
                case DomainModelValidationException ex:
                    ReportDomainModelInvalid(ex);
                    break;
            }
        }

        private void ReportConfigurationMultiple(DddMultipleConfigurationsFoundException e)
        {
            var foundFileNames = string.Join(", ", e.Files.Select(c => $"'{c}'"));
            var diagnostic = Diagnostic.Create(DddDiagnostics.ConfigurationMultipleError, Location.None, foundFileNames);
            ReportDiagnostic(diagnostic);
        }

        private void ReportConfigurationInvalid(DddConfigurationValidationException e)
        {
            foreach (var diagnostic in e.ErrorDiagnostics)
            {
                ReportDiagnostic(diagnostic);
            }
        }

        private void ReportSchemaInvalid(DddSchemaValidationException e)
        {
            foreach (var diagnostic in e.ErrorDiagnostics)
            {
                ReportDiagnostic(diagnostic);
            }
        }

        private void ReportDomainModelInvalid(DomainModelValidationException e)
        {
            foreach (var diagnostic in e.ErrorDiagnostics)
            {
                ReportDiagnostic(diagnostic);
            }
        }
        
    }
}