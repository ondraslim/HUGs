using Microsoft.CodeAnalysis;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public static class DddDiagnostics
    {
        public const string LoadErrorId = "HUGSDDD01";
        public const string ConfigurationMultipleErrorId = "HUGSDDD02";
        public const string SchemaInvalidErrorId = "HUGSDDD03";
        public const string SchemaInvalidValueErrorId = "HUGSDDD04";
        public const string ConfigurationInvalidErrorId = "HUGSDDD05";
        public const string ConfigurationInvalidValueErrorId = "HUGSDDD06";
        public const string DddModelInvalidErrorId = "HUGSDDD07";
        public const string DddModelDuplicatedNamesErrorId = "HUGSDDD08";

        internal static readonly DiagnosticDescriptor LoadError = new(
            id: LoadErrorId,
            title: "DDD Generator load failed",
            messageFormat: "DDD generator failed to load additional files",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor ConfigurationMultipleError = new(
            id: ConfigurationMultipleErrorId,
            title: "Multiple configurations found",
            messageFormat: "Expected only 1 configuration file, but found multiple files: {0}",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor SchemaInvalidError = new(
            id: SchemaInvalidErrorId,
            title: "Invalid Schema",
            messageFormat: "Schema '{0}' is not valid, DDD generator failed",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor SchemaInvalidValueError = new(
            id: SchemaInvalidValueErrorId,
            title: "Invalid schema value",
            messageFormat: "Schema contains invalid value '{0}' for property '{1}' in schema '{2}'",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor ConfigurationInvalidError = new(
            id: ConfigurationInvalidErrorId,
            title: "Invalid configuration",
            messageFormat: "Configuration '{0}' is not valid, DDD generator failed",
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

        internal static readonly DiagnosticDescriptor DddModelInvalidError = new(
            id: DddModelInvalidErrorId,
            title: "Invalid DDD model",
            messageFormat: "DDD model is invalid",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal static readonly DiagnosticDescriptor DddModelDuplicatedNamesError = new(
            id: DddModelDuplicatedNamesErrorId,
            title: "DDD model duplicates found",
            messageFormat: "DDD model contains duplicated objects with name '{0}'",
            category: "DddGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}