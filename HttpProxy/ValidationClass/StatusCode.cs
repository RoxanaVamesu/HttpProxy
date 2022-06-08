using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class StatusCode : IPattern
    {
        private readonly IPattern pattern;

        public StatusCode()
        {
            pattern = new Sequence(new IPattern[] { new Range('1', '5'), BasicRules.DIGIT, BasicRules.DIGIT });
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
