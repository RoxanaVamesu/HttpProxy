using System;
using Xunit;

namespace HttpProxy
{
    public class ValueTests
    {
        [Fact]
        public void TestSimpleValueExemple()
        {
            var text = "trafic.ro";
            var value = new Value();
            Assert.True(value.Match(text).Success);
        }

        [Fact]
        public void TestValueExempleWithSeparator()
        {
            var text = "text/htm,application/xhtml";
            var value = new Value();
            Assert.True(value.Match(text).Success);
        }

        [Fact]
        public void TestValueExempleWithString()
        {
            var text = "text,\"text\"";
            var value = new Value();
            Assert.True(value.Match(text).Success);
        }
    }
}
