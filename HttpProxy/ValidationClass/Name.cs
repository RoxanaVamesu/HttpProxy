using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Name : IPattern
    {
        private readonly IPattern pattern;

        public Name()
        {
            pattern = new Token();
        }

        public IMatch Match(string text)
        {
            var remainingText = text;
            var success = text.All(_ =>
            {
                var result = pattern.Match(text);
                remainingText = result.RemainingText;
                return result.Success;
            });
            return new Match(success, remainingText);
        }
    }
}
