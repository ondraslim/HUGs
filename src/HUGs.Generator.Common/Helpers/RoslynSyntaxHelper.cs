using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        }
}