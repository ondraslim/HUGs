using Microsoft.CodeAnalysis;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostics
    {
        public static readonly DiagnosticDescriptor ConfigurationDiagnostics = new(
            id: "HUGSDDDCONF01",
            title: "Multiple configurations found",
            messageFormat: "Expected only 1 configuration file, but found multiple files: {0}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}