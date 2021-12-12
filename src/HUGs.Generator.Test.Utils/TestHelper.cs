using System.Linq;

namespace HUGs.Generator.Test.Utils
{
    public static class TestHelper
    {
        public static string GetGeneratedFileClass(string generatedFile) 
            => generatedFile.Split(' ').SkipWhile(t => t != "class").Skip(1).First().Trim();
    }
}