using System;
using FluentAssertions;
using HUGs.Generator.Common.Helpers;
using NUnit.Framework;

namespace HUGs.Generator.Common.Tests
{
    public class ExceptionHelperTests
    {
        [Test]
        public void SimpleExceptionWithMessage_TheMessageIsReturned()
        {
            var ex = new Exception("test message");

            var result = ExceptionHelper.GetCompleteExceptionMessage(ex);

            result.Should().Contain("test message");
        }

        [Test]
        public void NestedExceptionsWithMessages_AllTheMessagesAreReturned()
        {
            var ex1 = new Exception("test message 1");
            var ex2 = new Exception("test message 2", ex1);
            var ex3 = new Exception("test message 3", ex2);

            var result1 = ExceptionHelper.GetCompleteExceptionMessage(ex1);
            var result2 = ExceptionHelper.GetCompleteExceptionMessage(ex2);
            var result3 = ExceptionHelper.GetCompleteExceptionMessage(ex3);

            result1.Should().Contain("test message");
            result2.Should()
                .Contain("test message 1")
                .And.Contain("test message 2");
            
            result3.Should()
                .Contain("test message 1")
                .And.Contain("test message 2")
                .And.Contain("test message 3");
        }
    }
}