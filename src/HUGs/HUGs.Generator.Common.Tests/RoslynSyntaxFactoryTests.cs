using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class RoslynSyntaxFactoryTests
    {
        private RoslynSyntaxFactory factory;

        [SetUp]
        public void Setup()
        {
            factory = new RoslynSyntaxFactory().CreateBuilder();
        }

        [Test]
        public void GivenRoslynSyntaxFactory_WhenAllInfoIsFilled_CorrectlyGeneratesCode()
        {
            factory.AddUsing("System");
            factory.AddNamespace("HUGs.Generator.Common.Tests");

            var sampleClass = PrepareSampleClassDeclarationSyntax();
            factory.AddClass(sampleClass);

            var actualCode = factory.Build();
            const string expectedCode = @"using System;

namespace HUGs.Generator.Common.Tests
{
    class TestClass
    {
        public void TestMethod()
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}";

            actualCode.Should().Be(expectedCode);
        }


        private ClassDeclarationSyntax PrepareSampleClassDeclarationSyntax()
        {
            var builder = new ClassBuilder("TestClass");

            var method = new MethodBuilder()
                .SetName("TestMethod")
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .AddBodyLine("Console.WriteLine(\"Hello World!\");")
                .Build();

            builder.AddMethod(method);

            return builder.Build();
        }
    }
}