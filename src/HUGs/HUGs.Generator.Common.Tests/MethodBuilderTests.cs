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
                .SetName("TestMethod")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedMethod = @"public void TestMethod()
{
}";

            actualMethod.Should().BeIgnoringLineEndings(expectedMethod);
        }

        [Test]
        public void GivenMethodWithSimpleBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedMethod = @"public void TestMethod()
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().BeIgnoringLineEndings(expectedMethod);
        }

        [Test]
        public void GivenMethodWithBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .AddBodyLine("System.Console.WriteLine(\"Another Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedMethod = @"public void TestMethod()
{
    System.Console.WriteLine(""Hello World!"");
    System.Console.WriteLine(""Another Hello World!"");
}";

            actualMethod.Should().BeIgnoringLineEndings(expectedMethod);
        }

        [Test]
        public void GivenMethodWithSimpleParam_CorrectlyGeneratesMethodWithParam()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddParameter("helloText", "string")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            const string expectedMethod = @"public void TestMethod(string helloText)
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().BeIgnoringLineEndings(expectedMethod);
        }

        [Test]
        public void GivenMethodWithTwoParams_CorrectlyGeneratesMethodWithParams()
        {
            var methodDeclaration = builder
                .SetAccessModifiers(SyntaxKind.PublicKeyword, SyntaxKind.VirtualKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddParameter("helloText", "string")
                .AddParameter("helloCount", "int")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.ToFullString();
            const string expectedMethod = @"public virtual void TestMethod(string helloText, int helloCount)
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().BeIgnoringLineEndings(expectedMethod);
        }
    }
    
}