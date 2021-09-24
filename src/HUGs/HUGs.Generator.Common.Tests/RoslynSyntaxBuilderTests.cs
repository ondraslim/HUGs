using CheckTestOutput;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class RoslynSyntaxBuilderTests
    {
        private RoslynSyntaxBuilder builder;
        private readonly OutputChecker check = new("TestResults");

        [SetUp]
        public void Setup()
        {
            builder = new RoslynSyntaxBuilder();
        }

        [Test]
        public void GivenRoslynSyntaxBuilder_WhenAllInfoIsFilled_CorrectlyGeneratesCode()
        {
            builder.AddUsing("System");
            builder.AddNamespace("HUGs.Generator.Common.Tests");

            var sampleClass = PrepareSampleClassDeclarationSyntax();
            builder.AddClass(sampleClass);

            var actualCode = builder.Build();
            check.CheckString(actualCode, fileExtension: "cs");
        }


        private static ClassDeclarationSyntax PrepareSampleClassDeclarationSyntax()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddConstructor(
                new[] { SyntaxKind.PublicKeyword }, 
                "TestClass",
                new ParameterSyntax[] { });

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .AddBodyLine("Console.WriteLine(\"Hello World!\");")
                .Build();

            builder.AddMethod(method);

            return builder.Build();
        }
    }
}