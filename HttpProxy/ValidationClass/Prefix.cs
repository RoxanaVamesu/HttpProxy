using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Prefix:IPattern
    {
        readonly string prefix;

        public Prefix(string prefix)
        {
            this.prefix = prefix;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < prefix.Length || !prefix.Contains(text.Substring(0, prefix.Length)))
            {
                return new Match(false, text);
            }

            return new Match(true, text.Substring(prefix.Length));
        }
    }
}
