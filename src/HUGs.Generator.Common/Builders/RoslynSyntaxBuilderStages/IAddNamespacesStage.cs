namespace HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages
{
    public interface IAddNamespacesStage
    {
        IAddNamespacesStage AddNamespaces(params string[] ns);
        string Build();
    }
}