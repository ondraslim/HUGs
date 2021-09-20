using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.Tests.Tools.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class MethodBuilderTests
    {
        private MethodBuilder builder;
        private readonly OutputChecker check = new("TestResults/MethodBuilder");

        [SetUp]
        public void Setup()
        {
            builder = new MethodBuilder();
        }

        [Test]
        public void GivenEmptyMethod_CorrectlyGeneratesEmptyMethod()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod1")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void GivenMethodWithSimpleBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod2")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void GivenMethodWithBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod3")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .AddBodyLine("System.Console.WriteLine(\"Another Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void GivenMethodWithSimpleParam_CorrectlyGeneratesMethodWithParam()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod4")
                .AddParameter("helloText", "string")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }

        [Test]
        public void GivenMethodWithTwoParams_CorrectlyGeneratesMethodWithParams()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.VirtualKeyword)
                .SetReturnType("void")
                .SetName("TestMethod5")
                .AddParameter("helloText", "string")
                .AddParameter("helloCount", "int")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.ToFullString();
            check.CheckString(actualMethod, fileExtension: "cs");
        }
    }
    
}