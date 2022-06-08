using System;
using Xunit;

namespace HttpProxy
{
    public class RequestURITests
    {
        [Fact]
        public void TestAbsoluteUri()
        {
            var text = "http://trafic.ro/";
            var uri = new RequestURI().Match(text);
            Assert.True(uri.Success);
            Assert.Equal("", uri.RemainingText);
        }

        [Fact]
        public void TestComlexUri()
        {
            var text = "http://www.ietf.org/rfc/rfc2396.txt";
            var uri = new RequestURI().Match(text);
            Assert.True(uri.Success);
            Assert.Equal("", uri.RemainingText);
        }
    }
}
