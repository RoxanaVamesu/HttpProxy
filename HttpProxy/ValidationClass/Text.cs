using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Text:IPattern
    {
        private readonly IPattern pattern;

        public Text(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            var remainingText = text;
            for(int i = 1; i <= text.Length; i++)
            {
                var result = pattern.Match(remainingText);
                if (!result.Success)
                {
                    return new Match(false, result.RemainingText);
                }

                remainingText = result.RemainingText;
            }

            return new Match(true, remainingText);
        }
    }
}
