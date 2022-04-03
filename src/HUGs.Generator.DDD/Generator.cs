using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Generators;
using Microsoft.CodeAnalysis;

namespace HUGs.Generator.DDD
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            DddGenerator.Initialize(context);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            DddGenerator.Execute(context);
        }
    }
}
