namespace HUGs.Generator.Common.Builders.RoslynSyntaxBuilderStages
{
    public interface IAddUsingsStage
    {
        IAddUsingsStage AddUsings(params string[] usings);
        IAddClassStage SetNamespace(string ns);

    }
}