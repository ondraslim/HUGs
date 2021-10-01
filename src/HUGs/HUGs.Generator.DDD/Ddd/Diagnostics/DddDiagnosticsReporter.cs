using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Exceptions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Diagnostics
{
    public class DddDiagnosticsReporter : DiagnosticsReporter
    {
        public DddDiagnosticsReporter(GeneratorExecutionContext generatorExecutionContext) 
            : base(generatorExecutionContext)
        {
        }

        public void ReportDiagnostic(LoadException e)
        {
            var diagnostic = e switch
            {
                AdditionalFileParseException ex => GetAdditionalFileParseDiagnostic(ex),
                DddMultipleConfigurationsFoundException ex => GetConfigurationMultipleDiagnostic(ex),
                DddConfigurationValidationException ex => GetConfigurationInvalidDiagnostic(ex),
                DddSchemaValidationException ex => GetSchemaInvalidDiagnostic(ex),
                DddModelValidationException ex => GetDddModelInvalidDiagnostic(ex),
                LoadException ex => GetLoadDiagnostic(ex),
            };

            ReportDiagnostic(diagnostic);
        }

        #region Diagnostic from Exceptions


        private static Diagnostic GetConfigurationMultipleDiagnostic(DddMultipleConfigurationsFoundException e)
        {
            var foundFileNames = string.Join(", ", e.Files.Select(c => $"'{c}'"));
            return Diagnostic.Create(DddDiagnostics.ConfigurationMultipleError, Location.None, foundFileNames);
        }

        private static Diagnostic GetConfigurationInvalidDiagnostic(DddConfigurationValidationException e)
        {
            return Diagnostic.Create(DddDiagnostics.ConfigurationInvalidError, Location.None, e.ConfigurationFile);
        }

        private static Diagnostic GetSchemaInvalidDiagnostic(DddSchemaValidationException e)
        {
            return Diagnostic.Create(DddDiagnostics.SchemaInvalidError, Location.None, e.SchemaFile);
        }

        private static Diagnostic GetDddModelInvalidDiagnostic(DddModelValidationException _)
        {
            return Diagnostic.Create(DddDiagnostics.DddModelInvalidError, Location.None);
        }


        private static Diagnostic GetLoadDiagnostic(LoadException _)
        {
            return Diagnostic.Create(DddDiagnostics.LoadError, Location.None);
        }

        #endregion

        public void ReportSchemaInvalidValue(string value, string property, string schema)
        {
            var diagnostic = Diagnostic.Create(DddDiagnostics.SchemaInvalidValueError, Location.None, value, property, schema);
            ReportDiagnostic(diagnostic);
        }

        public void ReportConfigurationInvalidValue(string value, string property)
        {
            var diagnostic = Diagnostic.Create(DddDiagnostics.ConfigurationInvalidValueError, Location.None, value, property);
            ReportDiagnostic(diagnostic);
        }

        public void ReportDuplicatedDddObjectNames(string duplicatedName)
        {
            var diagnostic = Diagnostic.Create(DddDiagnostics.DddModelDuplicatedNamesError, Location.None, duplicatedName);
            ReportDiagnostic(diagnostic);
        }
    }
}