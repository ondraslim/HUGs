using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Validation;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    public static class ConfigurationLoader
    {
        private static ConfigurationValidator _configurationValidator;

        private static void InitializeDependencies(GeneratorExecutionContext context)
        {
            _configurationValidator = new ConfigurationValidator(new DiagnosticReporter(context));
        }

        public static DddGeneratorConfiguration LoadConfiguration(GeneratorExecutionContext context)
        {
            InitializeDependencies(context);

            var configurationFile = GetDddConfiguration(context);
            return LoadConfiguration(configurationFile);
        }

        private static AdditionalText GetDddConfiguration(GeneratorExecutionContext context)
        {
            var configurations = context
                .AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".dddconfig", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (configurations.Count > 1)
            {
                throw new DddMultipleConfigurationsFoundException(configurations.Select(c => c.Path).ToList());
            }

            return configurations.FirstOrDefault();
        }

        private static DddGeneratorConfiguration LoadConfiguration(AdditionalText configurationFile)
        {
            var configuration = BuildConfiguration(configurationFile);
            if (!_configurationValidator.ValidateConfiguration(configuration))
            {
                throw new DddConfigurationValidationException(configurationFile.Path);
            }

            return configuration;
        }

        private static DddGeneratorConfiguration BuildConfiguration(AdditionalText configurationFile)
        {
            var configurationText = configurationFile?.GetText()?.ToString();

            if (string.IsNullOrWhiteSpace(configurationText))
            {
                return new DddGeneratorConfiguration();
            }

            try
            {
                var configuration = LoaderCommon.Deserialize<DddGeneratorConfiguration>(configurationText);
                return configuration;
            }
            catch (Exception e)
            {
                throw new AdditionalFileParseException($"Error occurred while parsing file: {configurationFile.Path}", configurationFile.Path, e);
            }
        }

    }
}