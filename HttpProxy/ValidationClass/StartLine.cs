using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class StartLine : IPattern
    {
        private readonly IPattern pattern;

        public StartLine()
        {
            this.pattern = new Choice(new IPattern[] { new RequestLine(), new StatusLine() });
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
