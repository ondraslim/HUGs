using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;
using System;

namespace HUGs.Generator.Common.Diagnostics
{
    /// <summary>
    /// Reports diagnostics by adding them to the compilation of generator execution context.
    /// </summary>
    public class DiagnosticsReporter
    {
        private readonly GeneratorExecutionContext generatorExecutionContext;

        public DiagnosticsReporter(GeneratorExecutionContext generatorExecutionContext)
        {
            this.generatorExecutionContext = generatorExecutionContext;
        }

        public virtual void ReportDiagnostic(Exception e)
        {
            switch (e)
            {
                case AdditionalFileParseException ex:
                    ReportAdditionalFileParse(ex);
                    break;
            }
        }

        protected void ReportDiagnostic(Diagnostic diagnostic)
        {
            generatorExecutionContext.ReportDiagnostic(diagnostic);
        }

        protected void ReportAdditionalFileParse(AdditionalFileParseException e)
        {
            var diagnostic = Diagnostic.Create(Diagnostics.AdditionalFileParseError, Location.None, e.FilePath, e.InnerException?.Message ?? e.Message);
            ReportDiagnostic(diagnostic);
        }

        public void ReportEmptyAdditionalFile(string additionalFilePath)
        {
            var diagnostic = Diagnostic.Create(Diagnostics.AdditionalFileEmptyWarning, Location.None, DiagnosticSeverity.Warning, additionalFilePath);
            ReportDiagnostic(diagnostic);
        }

    }
}