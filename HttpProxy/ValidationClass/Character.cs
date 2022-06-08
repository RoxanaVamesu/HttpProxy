using System;
using System.Collections.Generic;
using System.Text;

namespace HttpProxy
{
    class Character : IPattern
    {
        readonly char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && text[0] == pattern ? new Match(true, text[1..]) : new Match(false, text);
        }
    }
}
