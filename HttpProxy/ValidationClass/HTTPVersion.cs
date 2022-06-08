using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class HTTPVersion : IPattern
    {
        private readonly IPattern pattern;

        public HTTPVersion()
        {
            var patterns = new IPattern[] {new Character('H'),
                new Character('T'),
                new Character('T'),
                new Character('P'),
                new Character('/'),
                BasicRules.DIGIT,
                new Character('.'),
                BasicRules.DIGIT};
            pattern = new Sequence(patterns);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
