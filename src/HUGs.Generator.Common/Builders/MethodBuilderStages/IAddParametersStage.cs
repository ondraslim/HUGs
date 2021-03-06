using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HUGs.Generator.Common.Builders.MethodBuilderStages
{
    public interface IAddParametersStage
    {
        IAddParametersStage AddParameter(string paramName, string type);
        IAddBodyLineStage AddBodyLine(string line);
        IAddBodyLineStage AddBodyLine(StatementSyntax line);
        MethodDeclarationSyntax Build(bool methodHeaderOnly = false);
    }
}