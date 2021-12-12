using CheckTestOutput;
using HUGs.Generator.Common.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class MethodBuilderTests
    {
        private readonly OutputChecker check = new("TestResults");
        
        [Test]
        public void EmptyMethod_CorrectMethodGenerated()
        {
            var methodDeclaration = MethodBuilder.Create()
                .SetName("TestMethod1")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void SimpleBodyMethod_CorrectMethodGenerated()
        {
            var methodDeclaration = MethodBuilder.Create()
                .SetName("TestMethod2")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void BodyMethod_CorrectMethodGenerated()
        {
            var methodDeclaration = MethodBuilder.Create()
                .SetName("TestMethod3")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .AddBodyLine("System.Console.WriteLine(\"Another Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void SimpleParamMethod_CorrectMethodGenerated()
        {
            var methodDeclaration = MethodBuilder.Create()
                .SetName("TestMethod4")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .AddParameter("helloText", "string")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void TwoParamsMethod_CorrectMethodGenerated()
        {
            var methodDeclaration = MethodBuilder.Create()
                .SetName("TestMethod5")
                .SetReturnType("void")
                .SetAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.VirtualKeyword)
                .AddParameter("helloText", "string")
                .AddParameter("helloCount", "int")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }
    }
    
}