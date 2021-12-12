using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HUGs.Generator.Common.Builders.MethodBuilderStages
{
    public interface IAddBodyLineStage
    {
        IAddBodyLineStage AddBodyLine(string line);
        MethodDeclarationSyntax Build(bool methodHeaderOnly = false);
    }
}