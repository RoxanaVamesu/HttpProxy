using System;
using System.Collections.Generic;
using System.Text;

namespace HttpProxy
{
    class Sequence : IPattern
    {
        readonly IPattern[] patterns;

        public Sequence(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            string remainingText = text;
            foreach (var pattern in patterns)
            {
                var match = pattern.Match(remainingText);
                if (!match.Success)
                {
                    return new Match(false, text);
                }

                remainingText = match.RemainingText;
            }

            return new Match(true, remainingText);
        }
    }
}
