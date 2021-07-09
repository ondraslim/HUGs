using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace HUGs.Generator.Tests.Tools.Extensions
{
    public static class AssertionExtensions
    {
        public static AndConstraint<StringAssertions> BeIgnoringLineEndings(
            this StringAssertions stringAssertions, 
            string expected, 
            string because = "", 
            params object[] becauseArgs)
        {
            return stringAssertions.Subject.Replace("\r\n", "\n").Should().Be(expected.Replace("\r\n", "\n"));
        }

        public static AndConstraint<StringCollectionAssertions> ContainIgnoringLineEndings(
            this StringCollectionAssertions stringCollectionAssertions,
            IEnumerable<string> expected, 
            string because = "", 
            params object[] becauseArgs)
        {
            return stringCollectionAssertions.Subject.Select(s => s.Replace("\r\n", "\n"))
                .Should()
                .Contain(expected.Select(e => e.Replace("\r\n", "\n")));
        }
    }
}