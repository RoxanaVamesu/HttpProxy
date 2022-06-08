using System;

namespace HttpProxy
{
    public class String : IPattern
    {
        private readonly IPattern pattern;
        public String()
        {
            var inputChar = new IPattern[] { new Range(Convert.ToChar(0), Convert.ToChar(33)), 
                new Range(Convert.ToChar(35), Convert.ToChar(127)) };
            var patterns = new IPattern[] { new Character('\"'), new OneOrMore(new Choice(inputChar)), new Character('\"') };
            this.pattern = new Sequence(patterns);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
