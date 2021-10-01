using Microsoft.CodeAnalysis;

namespace HUGs.Generator.Common.Diagnostics
{
    public static class Diagnostics
    {
        public const string AdditionalFileEmptyWarningId = "HUGS01";
        public const string AdditionalFileParseErrorId = "HUGS02";
        public const string GeneratedCodeSyntaxErrorId = "HUGS03";

        internal static readonly DiagnosticDescriptor AdditionalFileEmptyWarning = new(
            id: AdditionalFileEmptyWarningId,
            title: "Empty AdditionalFile found",
            messageFormat: "Found empty AdditionalFile '{0}'",
            category: "Generator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor AdditionalFileParseError = new(
            id: AdditionalFileParseErrorId,
            title: "AdditionalFile parse fail",
            messageFormat: "Could not parse AdditionalFile '{0}' with error: {1}",
            category: "Generator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor GeneratedCodeSyntaxError = new(
            id: GeneratedCodeSyntaxErrorId,
            title: "Generated code syntax error",
            messageFormat: "Generated code for '{0}' is not valid",
            category: "Generator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}