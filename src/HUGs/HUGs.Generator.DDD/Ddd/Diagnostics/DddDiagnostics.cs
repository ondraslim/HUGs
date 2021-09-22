using Microsoft.CodeAnalysis;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostics
    {
        public static readonly DiagnosticDescriptor ConfigurationConflictError = new(
            id: "HUGSDDDCONF01",
            title: "Multiple configurations found",
            messageFormat: "Expected only 1 configuration file, but found multiple files: {0}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor AdditionalFileParseError = new(
            id: "HUGSDDDCONF02",
            title: "AdditionalFile parse error",
            messageFormat: "Could not parse AdditionalFile '{0}' with error: {1}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor EmptyAdditionalFileWarning = new(
            id: "HUGSDDDCONF03",
            title: "Empty AdditionalFile found",
            messageFormat: "Found empty AdditionalFile '{0}'",
            category: "DddGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    }
}