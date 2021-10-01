using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostic
    {
        public const string MultipleConfigurationsErrorId = "HUGSDDDAF01";
        public const string AdditionalFileParseErrorId = "HUGSDDDAF02";
        public const string EmptyAdditionalFileWarningId = "HUGSDDDAF03";
        public const string SchemaInvalidValueErrorId = "HUGSDDDAF04";
        public const string ConfigurationInvalidValueErrorId = "HUGSDDDAF05";
        public const string SchemaInvalidErrorId = "HUGSDDDAF06";
        public const string ConfigurationInvalidErrorId = "HUGSDDDAF07";
        public const string LoadErrorId = "HUGSDDDAF08";
        public const string DuplicatedDddObjectNamesErrorId = "HUGSDDDAF09";
        public const string InvalidDddModelErrorId = "HUGSDDDAF10";

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
            id: SchemaInvalidValueErrorId,
            title: "Invalid schema value",
            messageFormat: "Schema contains invalid value '{0}' for property '{1}' in schema '{2}'",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);


        internal static readonly DiagnosticDescriptor InvalidSchemaError = new(
            id: SchemaInvalidErrorId,
            title: "Invalid Schema",
            messageFormat: "Schema '{0}' is not valid, DDD generator failed",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor ConfigurationInvalidValueError = new(
            id: ConfigurationInvalidValueErrorId,
            title: "Invalid configuration value",
            messageFormat: "Configuration contains invalid value '{0}' for property '{1}' in configuration",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor InvalidConfigurationError = new(
            id: ConfigurationInvalidErrorId,
            title: "Invalid configuration",
            messageFormat: "Configuration '{0}' is not valid, DDD generator failed",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor LoadError = new(
            id: LoadErrorId,
            title: "DDD Generator load failed",
            messageFormat: "DDD generator failed to load additional files",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor DuplicatedDddObjectNamesError = new(
            id: DuplicatedDddObjectNamesErrorId,
            title: "DDD model contains duplicates",
            messageFormat: "DDD model contains duplicated objects with name '{0}'",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor InvalidDddModelError = new(
            id: InvalidDddModelErrorId,
            title: "Invalid DDD model",
            messageFormat: "DDD model is invalid",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static Diagnostic ExceptionToDiagnosticConverter(DddLoadException e) => e switch
        {
            AdditionalFileParseException ex => GetAdditionalFileParseDiagnostic(ex),
            DddMultipleConfigurationsFoundException ex => GetMultipleConfigurationsDiagnostic(ex),
            DddConfigurationValidationException ex => GetInvalidConfigurationDiagnostic(ex),
            DddSchemaValidationException ex => GetInvalidSchemaDiagnostic(ex),
            DddModelValidationException ex => GetInvalidDddModelError(ex),
            DddLoadException ex => GetLoadErrorDiagnostic(ex),
        };

        #region Diagnostic from Exception

        private static Diagnostic GetAdditionalFileParseDiagnostic(AdditionalFileParseException e)
        {
            return Diagnostic.Create(AdditionalFileParseError, Location.None, e.FilePath, e.InnerException?.Message ?? e.Message);
        }

        private static Diagnostic GetMultipleConfigurationsDiagnostic(DddMultipleConfigurationsFoundException e)
        {
            var foundFileNames = string.Join(", ", e.Files.Select(c => $"'{c}'"));
            return Diagnostic.Create(MultipleConfigurationsError, Location.None, foundFileNames);
        }

        private static Diagnostic GetInvalidConfigurationDiagnostic(DddConfigurationValidationException e)
        {
            return Diagnostic.Create(InvalidConfigurationError, Location.None, e.ConfigurationFile);
        }

        private static Diagnostic GetInvalidSchemaDiagnostic(DddSchemaValidationException e)
        {
            return Diagnostic.Create(InvalidSchemaError, Location.None, e.SchemaFile);
        }

        private static Diagnostic GetInvalidDddModelError(DddModelValidationException _)
        {
            return Diagnostic.Create(InvalidDddModelError, Location.None);
        }

        private static Diagnostic GetLoadErrorDiagnostic(DddLoadException _)
        {
            return Diagnostic.Create(LoadError, Location.None);
        }

        #endregion

        #region Other diagnostic

        internal static Diagnostic GetSchemaInvalidValueDiagnostic(string value, string property, string schema)
        {
            return Diagnostic.Create(InvalidValueSchemaError, Location.None, value, property, schema);
        }

        internal static Diagnostic GetConfigurationInvalidValueDiagnostic(string value, string property)
        {
            return Diagnostic.Create(ConfigurationInvalidValueError, Location.None, value, property);
        }

        internal static Diagnostic GetDuplicatedDddObjectNamesDiagnostic(string duplicatedName)
        {
            return Diagnostic.Create(DuplicatedDddObjectNamesError, Location.None, duplicatedName);
        }

        #endregion
    }
}