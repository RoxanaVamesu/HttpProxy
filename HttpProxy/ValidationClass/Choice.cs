using System;
using System.Collections.Generic;
using System.Text;

namespace HttpProxy
{
    class Choice : IPattern
    {
        private readonly IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i].Match(text).Success)
                {
                    return new Match(true, patterns[i].Match(text).RemainingText);
                }
            }

            return new Match(false, text);
        }
    }
}
