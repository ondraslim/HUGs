using FluentAssertions;
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
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            var expected = @"public void TestMethod()
{
}";

            actualMethod.Should().Be(expected);
        }

        [Test]
        public void GivenMethodWithSimpleBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            var expected = @"public void TestMethod()
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().Be(expected);
        }

        [Test]
        public void GivenMethodWithBody_CorrectlyGeneratesMethodWithBody()
        {
            var methodDeclaration = builder
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .AddBodyLine("System.Console.WriteLine(\"Another Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            var expected = @"public void TestMethod()
{
    System.Console.WriteLine(""Hello World!"");
    System.Console.WriteLine(""Another Hello World!"");
}";

            actualMethod.Should().Be(expected);
        }

        [Test]
        public void GivenMethodWithSimpleParam_CorrectlyGeneratesMethodWithParam()
        {
            var methodDeclaration = builder
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddParameter("helloText", "string")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.NormalizeWhitespace().ToFullString();
            var expected = @"public void TestMethod(string helloText)
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().Be(expected);
        }

        [Test]
        public void GivenMethodWithTwoParams_CorrectlyGeneratesMethodWithParams()
        {
            var methodDeclaration = builder
                .SetAccessModifier(SyntaxKind.PublicKeyword)
                .SetReturnType("void")
                .SetName("TestMethod")
                .AddParameter("helloText", "string")
                .AddParameter("helloCount", "int")
                .AddBodyLine("System.Console.WriteLine(\"Hello World!\");")
                .Build();

            var actualMethod = methodDeclaration.ToFullString();
            var expected = @"public void TestMethod(string helloText, int helloCount)
{
    System.Console.WriteLine(""Hello World!"");
}";

            actualMethod.Should().Be(expected);
        }
    }
    
}