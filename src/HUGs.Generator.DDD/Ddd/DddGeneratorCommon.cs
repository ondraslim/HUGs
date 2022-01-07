using HUGs.Generator.Common.Builders;
using HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    internal static class DddGeneratorCommon
    {
        /// <summary>
        /// Adds common usings to given SyntaxBuilder
        /// </summary>
        public static void AddUsings(
            IAddUsingsStage syntaxBuilder,
            DddGeneratorConfiguration generatorConfiguration)
        {
            syntaxBuilder.AddUsings(
                "System",
                "System.Linq",
                "System.Collections.Generic",
                "HUGs.Generator.DDD.Framework.BaseModels",
                "HUGs.Generator.DDD.Framework.Mapping");

            syntaxBuilder.AddUsings(generatorConfiguration.AdditionalUsings.ToArray());

            foreach (DddObjectKind kind in Enum.GetValues(typeof(DddObjectKind)))
            {
                syntaxBuilder.AddUsings(generatorConfiguration.GetTargetNamespaceForKind(kind));
            }

            syntaxBuilder.AddUsings(generatorConfiguration.TargetNamespaces.DbEntity);
            syntaxBuilder.AddUsings(generatorConfiguration.TargetNamespaces.Mapper);
        }

        /// <summary>
        /// Creates ParameterSyntax objects based on DDD object properties
        /// </summary>
        public static ParameterSyntax[] CreateParametersFromProperties(DddObjectProperty[] properties)
        {
            var parameters = properties
                .Where(p => !p.Computed)
                .Select(p =>
                {
                    if (p.ResolvedType is DddCollectionType collectionType)
                    {
                        return RoslynSyntaxHelper.CreateParameterSyntax(collectionType.ToString("IEnumerable"), p.Name);
                    }

                    return RoslynSyntaxHelper.CreateParameterSyntax(p.ResolvedType.ToString(), p.Name);
                })
                .ToArray();

            return parameters;
        }

        /// <summary>
        /// Generates class field/property assignment statements for an array of DDD object properties.
        /// </summary>
        public static string[] CreateAssignmentStatementsFromProperties(DddObjectProperty[] properties)
        {
            var linesOfCode = properties
                .Where(p => !p.Computed)
                .Select(p => p.ResolvedType is DddCollectionType
                    ? $"this.{p.PrivateName} = {p.Name}{(p.ResolvedType.IsNullable ? "?" : "")}.ToList();"
                    : $"this.{p.Name} = {p.Name};")
                .ToArray();

            return linesOfCode;
        }

        /// <summary>
        /// Adds a list of properties to a class representing a DDD object
        /// </summary>
        public static void AddDddClassProperties(ClassBuilder classBuilder, IEnumerable<DddObjectProperty> properties, bool withPrivateSetter = false)
        {
            foreach (var property in properties)
            {
                if (property.ResolvedType is DddCollectionType collectionType)
                {
                    classBuilder
                        .AddField(collectionType.ToString(arrayType: "List"), property.PrivateName, SyntaxKind.PrivateKeyword)
                        .AddGetOnlyPropertyWithBackingField(
                            collectionType.ToString(arrayType: "IReadOnlyList"), property.Name, property.PrivateName, SyntaxKind.PublicKeyword);
                }
                else
                {
                    if (withPrivateSetter)
                    {
                        classBuilder.AddPropertyWithPrivateSetter(property.ResolvedType.ToString(), property.Name, SyntaxKind.PublicKeyword);
                    }
                    else
                    {
                        classBuilder.AddGetOnlyProperty(property.ResolvedType.ToString(), property.Name, SyntaxKind.PublicKeyword);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a list of properties to a class representing a DB entity
        /// </summary>
        public static void AddDbEntityClassProperties(ClassBuilder classBuilder, IEnumerable<DddObjectProperty> properties)
        {
            foreach (var property in properties.Where(p => !p.Computed))
            { 
                classBuilder.AddFullProperty(property.ResolvedType.ToString(), property.Name, SyntaxKind.PublicKeyword);
            }
        }

        /// <summary>
        /// Creates a header of 'OnInitialized' method
        /// </summary>
        public static MethodDeclarationSyntax BuildOnInitializedMethod()
        {
            return MethodBuilder.Create()
                .SetName("OnInitialized")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PartialKeyword)
                .Build(methodHeaderOnly: true);
        }

    }
}