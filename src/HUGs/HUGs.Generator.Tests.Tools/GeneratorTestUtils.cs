using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace HUGs.Generator.Tests.Tools
{
    public static class GeneratorTestUtils
    {
        public static void RunGenerator(
            GeneratorDriver driver,
            Compilation inputCompilation,
            out ImmutableArray<Diagnostic> diagnostics, 
            out ImmutableArray<string> generatedFileTexts)
        {
            driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !inputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();
        }
    }
}