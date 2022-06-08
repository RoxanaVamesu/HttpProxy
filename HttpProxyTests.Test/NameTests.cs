using System;
using Xunit;

namespace HttpProxy
{
    public class NameTests
    {
        [Fact]
        public void TestSimpleNameExemple()
        {
            var text = "Connection";
            var token = new Name();
            Assert.True(token.Match(text).Success);
        }

        [Fact]
        public void TestNameExempleWithLetterAndOtherChar()
        {
            var text = "Transfer-Encoding";
            var token = new Name();
            Assert.True(token.Match(text).Success);
        }

        [Fact]
        public void TestNameExempleWithLetterAndAnacceptedChar()
        {
            var text = " Transfer Encoding";
            var token = new Name();
            Assert.False(token.Match(text).Success);
        }
    }
}
