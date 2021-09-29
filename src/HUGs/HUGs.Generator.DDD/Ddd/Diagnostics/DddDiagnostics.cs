using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostics
    {
        public const string MultipleConfigurationsErrorId = "HUGSDDDAF01";
        public const string AdditionalFileParseErrorId = "HUGSDDDAF02";
        public const string EmptyAdditionalFileWarningId = "HUGSDDDAF03";
        public const string InvalidValueSchemaErrorId = "HUGSDDDAF04";

        internal static readonly DiagnosticDescriptor MultipleConfigurationsError = new(
            id: MultipleConfigurationsErrorId,
            title: "Multiple configurations found",
            messageFormat: "Expected only 1 configuration file, but found multiple files: {0}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor AdditionalFileParseError = new(
            id: AdditionalFileParseErrorId,
            title: "AdditionalFile parse error",
            messageFormat: "Could not parse AdditionalFile '{0}' with error: {1}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor EmptyAdditionalFileWarning = new(
            id: EmptyAdditionalFileWarningId,
            title: "Empty AdditionalFile found",
            messageFormat: "Found empty AdditionalFile '{0}'",
            category: "DddGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor InvalidValueSchemaError = new(
            id: InvalidValueSchemaErrorId,
            title: "Invalid schema value",
            messageFormat: "Schema contains invalid value '{0}' for property '{1}' in schema '{2}'",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static Diagnostic ExceptionToDiagnosticConverter(Exception e) => e switch
        {
            AdditionalFileParseException afp => GetAdditionalFileParseExceptionToDiagnostic(afp),
            DddMultipleConfigurationsFoundException dmcf => GetMultipleConfigurationsExceptionToDiagnostic(dmcf),
            _ => throw new ArgumentOutOfRangeException(nameof(e), e, "Cannot convert exception to diagnostic.")
        };

        private static Diagnostic GetAdditionalFileParseExceptionToDiagnostic(AdditionalFileParseException e)
        {
            return Diagnostic.Create(AdditionalFileParseError, Location.None, e.FilePath, e.InnerException?.Message ?? e.Message);
        }

        private static Diagnostic GetMultipleConfigurationsExceptionToDiagnostic(DddMultipleConfigurationsFoundException e)
        {
            var foundFileNames = string.Join(", ", e.Files.Select(c => $"'{c}'"));
            return Diagnostic.Create(MultipleConfigurationsError, Location.None, foundFileNames);
        }

        internal static Diagnostic GetInvalidSchemaDiagnostic(string value, string property, string schema)
        {
            return Diagnostic.Create(InvalidValueSchemaError, Location.None, value, property, schema);
        }
    }
}