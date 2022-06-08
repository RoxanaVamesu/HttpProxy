using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class StatusLine :IPattern
    {
        private readonly IPattern pattern;

        public StatusLine()
        {
            var patterns = new IPattern[] { new HTTPVersion(), BasicRules.SP, new StatusCode(), BasicRules.SP, new ReasonPhrase()};
            this.pattern = new Sequence(patterns);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
