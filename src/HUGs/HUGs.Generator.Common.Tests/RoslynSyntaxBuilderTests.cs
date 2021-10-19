using CheckTestOutput;
using HUGs.Generator.Common.Builders;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class RoslynSyntaxBuilderTests
    {
        private readonly OutputChecker check = new("TestResults");


        [Test]
        public void RAllInfoFilled_GeneratedCorrectly()
        {
            var sampleClass = PrepareSampleClassDeclarationSyntax();

            var actualCode = RoslynSyntaxBuilder.Create()
                .AddUsings("System")
                .SetNamespace("HUGs.Generator.Common.Tests")
                .AddClass(sampleClass)
                .Build();

            check.CheckString(actualCode, fileExtension: "cs");
        }


        private static ClassDeclarationSyntax PrepareSampleClassDeclarationSyntax()
        {

            var method = MethodBuilder.Create()
                .SetName("TestMethod")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddBodyLine("Console.WriteLine(\"Hello World!\");")
                .Build();

            return ClassBuilder.Create()
                .SetClassName("TestClass")
                .AddConstructor("TestClass", accessModifiers: new[] { SyntaxKind.PublicKeyword })
                .AddMethod(method)
                .Build();
        }
    }
}