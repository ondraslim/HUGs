using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;

namespace HUGs.Generator.Common.Diagnostics
{
    public class DiagnosticsReporter
    {
        private readonly GeneratorExecutionContext generatorExecutionContext;

        public DiagnosticsReporter(GeneratorExecutionContext generatorExecutionContext)
        {
            this.generatorExecutionContext = generatorExecutionContext;
        }

        protected void ReportDiagnostic(Diagnostic diagnostic)
        {
            generatorExecutionContext.ReportDiagnostic(diagnostic);
        }

        protected static Diagnostic GetAdditionalFileParseDiagnostic(AdditionalFileParseException e)
        {
            return Diagnostic.Create(Diagnostics.AdditionalFileParseError, Location.None, e.FilePath, e.InnerException?.Message ?? e.Message);
        }

        public void ReportEmptyAdditionalFile(string additionalFilePath)
        {
            var diagnostic = Diagnostic.Create(Diagnostics.AdditionalFileEmptyWarning, Location.None, DiagnosticSeverity.Warning, additionalFilePath);
            ReportDiagnostic(diagnostic);
        }

        public void ReportGeneratedCodeValidationError(string sourceCodeFileName)
        {
            var diagnostic = Diagnostic.Create(Diagnostics.AdditionalFileEmptyWarning, Location.None, DiagnosticSeverity.Error, sourceCodeFileName);
            ReportDiagnostic(diagnostic);
        }

    }
}