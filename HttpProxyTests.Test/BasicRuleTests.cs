using System;
using Xunit;

namespace HttpProxy
{
    public class BasicRuleTests
    {
        [Fact]
        public void TestAbsoluteUri()
        {
            var text = "trafic";
            var domainlabel = BasicRules.Domainlabel.Match(text);
            Assert.True(domainlabel.Success);
            Assert.Equal("", domainlabel.RemainingText);
        }
    }
}
