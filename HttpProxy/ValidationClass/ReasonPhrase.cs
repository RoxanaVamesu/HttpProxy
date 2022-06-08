using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class ReasonPhrase : IPattern
    {
        private readonly IPattern pattern;

        public ReasonPhrase()
        {
            pattern = new Many(BasicRules.TEXT);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
