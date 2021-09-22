using Microsoft.CodeAnalysis;

namespace HUGs.Generator.Common.Diagnostics
{
    public class DiagnosticReporter
    {
        private readonly GeneratorExecutionContext generatorExecutionContext;

        public DiagnosticReporter(GeneratorExecutionContext generatorExecutionContext)
        {
            this.generatorExecutionContext = generatorExecutionContext;
        }

        public void ReportDiagnostic(Diagnostic diagnostic)
        {
            generatorExecutionContext.ReportDiagnostic(diagnostic);
        }
    }
}