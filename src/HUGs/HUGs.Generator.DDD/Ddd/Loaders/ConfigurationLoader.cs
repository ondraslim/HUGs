﻿using HUGs.Generator.Common.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using HUGs.Generator.DDD.Ddd.Models.Configuration;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    public static class ConfigurationLoader
    {
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
            // TODO: validate configuration
            //if (configuration is not null)
            //{
            //}

            return configuration;
        }

        private static DddGeneratorConfiguration BuildConfiguration(AdditionalText configurationFile)
        {
            var configurationText = configurationFile?.GetText()?.ToString();

            if (string.IsNullOrWhiteSpace(configurationText)) return null;

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