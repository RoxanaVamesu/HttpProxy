using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class RequestLine : IPattern 
    {
        private readonly IPattern pattern;

        public RequestLine()
        {
            var patterns = new IPattern[]
            {
                new Method(), BasicRules.SP, new RequestURI(), BasicRules.SP, new HTTPVersion()
            };
            this.pattern = new Sequence(patterns);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
