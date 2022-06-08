using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    class Separator : IPattern
    {
        private readonly IPattern pattern;

        public Separator()
        {
            var patterns = new IPattern[] {
                new Character( Convert.ToChar(9)),
                new Character( Convert.ToChar(32)),
                new Character(Convert.ToChar(34)),
                new Range(Convert.ToChar(40), Convert.ToChar(41)),
                new Character(Convert.ToChar(44)),
                new Character(Convert.ToChar(47)),
                new Range(Convert.ToChar(58), Convert.ToChar(64)),
                new Range(Convert.ToChar(91), Convert.ToChar(93)),
                new Character(Convert.ToChar(123)),
                new Character(Convert.ToChar(125))};
            this.pattern = new Choice(patterns);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
