using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HUGs.Generator.Common.Builders.MethodBuilderStages
{
    public interface ISetAccessModifiersStage
    {
        IAddParametersStage SetAccessModifiers(params SyntaxKind[] modifiers);
        MethodDeclarationSyntax Build(bool methodHeaderOnly = false);
    }
}