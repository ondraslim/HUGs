using HUGs.Generator.Common;
using HUGs.Generator.Common.Helpers;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.DDD.Ddd
{
    internal static class DddGeneratorCommon
    {
        public static void AddUsings(
            RoslynSyntaxBuilder syntaxBuilder,
            DddGeneratorConfiguration generatorConfiguration)
        {
            syntaxBuilder.AddUsings(
                "System",
                "System.Linq",
                "System.Collections.Generic",
                "HUGs.Generator.DDD.BaseModels");

            syntaxBuilder.AddUsings(generatorConfiguration.AdditionalUsings.ToArray());
        }

        public static ParameterSyntax[] CreateParametersFromProperties(DddObjectProperty[] properties)
        {
            var parameters = properties
                .Where(p => !p.Computed)
                .Select(p => RoslynSyntaxHelper.CreateParameterSyntax(
                    p.IsArrayProperty ? $"IEnumerable<{p.FullType}>" : p.FullType,
                    p.Name))
                .ToArray();

            return parameters;
        }

        public static string[] CreateAssignmentStatementsFromProperties(DddObjectProperty[] properties)
        {
            var linesOfCode = properties
                .Where(p => !p.Computed)
                .Select(p => p.IsArrayProperty
                    ? $"this.{p.PrivateName} = {p.Name}.ToList();"
                    : $"this.{p.Name} = {p.Name};")
                .ToArray();

            return linesOfCode;
        }

        public static void AddClassProperties(ClassBuilder classBuilder, IEnumerable<DddObjectProperty> properties, bool withPrivateSetter = false)
        {
            foreach (var property in properties)
            {
                if (property.IsArrayProperty)
                {
                    classBuilder
                        .AddField($"List<{property.TypeWithoutArray}>", property.PrivateName, SyntaxKind.PrivateKeyword)
                        .AddGetOnlyPropertyWithBackingField($"IReadOnlyList<{property.TypeWithoutArray}>", property.Name, property.PrivateName, new[]
                        {
                            SyntaxKind.PublicKeyword
                        });
                }
                else
                {
                    if (withPrivateSetter)
                    {
                        classBuilder.AddPropertyWithPrivateSetter(property.FullType, property.Name, SyntaxKind.PublicKeyword);
                    }
                    else
                    {
                        classBuilder.AddGetOnlyProperty(property.FullType, property.Name, SyntaxKind.PublicKeyword);
                    }
                }
            }
        }

        public static MethodDeclarationSyntax BuildOnInitializedMethod()
        {
            var methodBuilder = new MethodBuilder()
                .SetAccessModifiers(SyntaxKind.PartialKeyword)
                .SetReturnType("void")
                .SetName("OnInitialized");

            return methodBuilder.Build(methodHeaderOnly: true);
        }

    }
}