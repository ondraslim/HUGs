using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages
{
    public interface IAddClassStage
    {
        IAddClassStage AddClass(ClassDeclarationSyntax classDeclaration);
        string Build();
    }
}