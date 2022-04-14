using HUGs.Generator.Common.Exceptions;
using HUGs.Generator.DDD.Ddd.Exceptions;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using HUGs.Generator.DDD.Validators;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    /// <summary>
    /// Takes care of loading a DDD generator configuration based on a .dddconfig file.
    /// </summary>
    public static class ConfigurationLoader
    {
        /// <summary>
        /// Loads DDD generator configuration.
        /// Only one configuration file is allowed!
        /// </summary>
        public static DddGeneratorConfiguration LoadConfiguration(GeneratorExecutionContext context)
        {
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
            ConfigurationValidator.ValidateConfiguration(configuration);
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
                throw new AdditionalFileParseException(configurationFile.Path, $"{e}", e);
            }
        }

    }
}