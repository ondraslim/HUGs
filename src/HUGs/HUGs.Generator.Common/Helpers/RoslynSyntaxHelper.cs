using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HUGs.Generator.Common.Helpers
{
    public static class RoslynSyntaxHelper
    {
        public static ParameterSyntax CreateParameterSyntax(string type, string name)
        {
            return SyntaxFactory
                .Parameter(SyntaxFactory.Identifier(name))
                .WithType(SyntaxFactory.ParseTypeName(type));
        }

        // GetTypeSyntax methods inspired by this: https://gist.github.com/frankbryce/a4ee2bf799ab3878ae91
        public static TypeSyntax GetTypeSyntax(string identifier)
        {
            return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(identifier));
        }

        public static TypeSyntax GetTypeSyntax(string identifier, params string[] arguments)
        {
            return GetTypeSyntax(identifier, arguments.Select(GetTypeSyntax).ToArray());
        }

        public static TypeSyntax GetTypeSyntax(string identifier, params TypeSyntax[] arguments)
        {
            return
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier(identifier),
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList(
                            arguments.Select(
                                x =>
                                {
                                    if (x is GenericNameSyntax genX)
                                    {
                                        return
                                            GetTypeSyntax(
                                                genX.Identifier.ToString(),
                                                genX.TypeArgumentList.Arguments.ToArray()
                                            );
                                    }

                                    return x;
                                }
                            )
                        )
                    )
                );
        }
    }
}