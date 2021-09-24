using System;
using System.Linq;
using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostics
    {
        public const string MultipleConfigurationsErrorId = "HUGSDDDCONF01";
        public const string AdditionalFileParseErrorId = "HUGSDDDCONF02";
        public const string EmptyAdditionalFileWarningId = "HUGSDDDCONF03";

        public static readonly DiagnosticDescriptor MultipleConfigurationsError = new(
            id: MultipleConfigurationsErrorId,
            title: "Multiple configurations found",
            messageFormat: "Expected only 1 configuration file, but found multiple files: {0}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor AdditionalFileParseError = new(
            id: AdditionalFileParseErrorId,
            title: "AdditionalFile parse error",
            messageFormat: "Could not parse AdditionalFile '{0}' with error: {1}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor EmptyAdditionalFileWarning = new(
            id: EmptyAdditionalFileWarningId,
            title: "Empty AdditionalFile found",
            messageFormat: "Found empty AdditionalFile '{0}'",
            category: "DddGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);


        public static Diagnostic ExceptionToDiagnosticConverter(Exception e) => e switch
        {
            AdditionalFileParseException afp => AdditionalFileParseExceptionToDiagnostic(afp),
            DddMultipleConfigurationsFoundException dmcf => MultipleConfigurationsExceptionToDiagnostic(dmcf),
            _ => throw new ArgumentOutOfRangeException(nameof(e), e, "Cannot convert exception to diagnostic.")
        };

        private static Diagnostic AdditionalFileParseExceptionToDiagnostic(AdditionalFileParseException e)
        {
            return Diagnostic.Create(AdditionalFileParseError, Location.None, DiagnosticSeverity.Error, e.FilePath, e.InnerException?.Message ?? e.Message);
        }

        private static Diagnostic MultipleConfigurationsExceptionToDiagnostic(DddMultipleConfigurationsFoundException e)
        {
            var foundFileNames = string.Join(", ", e.Files.Select(c => $"'{c}'"));
            return Diagnostic.Create(MultipleConfigurationsError, Location.None, foundFileNames);
        }
    }
}