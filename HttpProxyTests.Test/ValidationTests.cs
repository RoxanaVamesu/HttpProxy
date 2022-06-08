using System;
using Xunit;

namespace HttpProxy
{
    public class ValidationTests
    {
        [Fact]
        public void TestRequestStartLine()
        {
            var text = "GET http://trafic.ro/ HTTP/1.1";
            var header = new Validator(text);
            Assert.True(header.CheckStartLine());
        }

        [Fact]
        public void TestResponseStartLine()
        {
            var text = "HTTP/1.1 200 OK";
            var header = new Validator(text);
            Assert.True(header.CheckStartLine());
        }
    }
}
