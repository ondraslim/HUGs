using FluentAssertions;
using HUGs.Generator.Tests.Tools.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class RoslynSyntaxBuilderTests
    {
        private RoslynSyntaxBuilder builder;

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
            const string expectedCode = @"using System;

namespace HUGs.Generator.Common.Tests
{
    class TestClass
    {
        public TestClass()
        {
        }

        public void TestMethod()
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}";

            actualCode.Should().BeIgnoringLineEndings(expectedCode);
        }


        private static ClassDeclarationSyntax PrepareSampleClassDeclarationSyntax()
        {
            var builder = new ClassBuilder("TestClass");

            builder.AddConstructor(
                new[] { SyntaxKind.PublicKeyword }, 
                "TestClass",
                new ParameterSyntax[] { },
                new string[] { });

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