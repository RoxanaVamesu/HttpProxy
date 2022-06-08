using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Token : IPattern
    {
        private readonly IPattern pattern;

        public Token()
        {
            var pattern = new IPattern[] { new Character(Convert.ToChar(33)),
                new Range(Convert.ToChar(35), Convert.ToChar(39)), 
                new Range(Convert.ToChar(42), Convert.ToChar(43)), 
                new Range(Convert.ToChar(45), Convert.ToChar(46)), 
                new Range(Convert.ToChar(48), Convert.ToChar(57)),                
                new Range(Convert.ToChar(65), Convert.ToChar(90)),
                new Range(Convert.ToChar(94), Convert.ToChar(122)),
                new Character(Convert.ToChar(124))};
            this.pattern = new Choice(pattern);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
