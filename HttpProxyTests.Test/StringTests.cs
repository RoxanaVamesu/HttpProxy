using System;
using Xunit;

namespace HttpProxy
{
    public class StringTests
    {
        [Fact]
        public void TestSimpleString()
        {
            var text = "\"trafic.ro\"";
            var @string = new String().Match(text);
            Assert.True(@string.Success);
            Assert.Equal("", @string.RemainingText);
        }

        [Fact]
        public void TestValueThatContainsAStringValueAtTheBiginOfTheText()
        {
            var text = "\"trafic.ro\"abc";
            var @string = new String().Match(text);
            Assert.True(@string.Success);
            Assert.Equal("abc", @string.RemainingText);
        }

        [Fact]
        public void TestValueThatContainsAStringValueInTheMiddleOfTheText()
        {
            var text = "abc\"trafic.ro\"abc";
            var @string = new String().Match(text);
            Assert.False(@string.Success);
            Assert.Equal(text, @string.RemainingText);
        }

        [Fact]
        public void TestValueThatContainsAStringValueThatDoNotEndWithQuoted()
        {
            var text = "\"trafic.ro";
            var @string = new String().Match(text);
            Assert.False(@string.Success);
            Assert.Equal(text, @string.RemainingText);
        }
    }
}
