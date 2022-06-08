using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class RequestURI:IPattern
    {
        private readonly IPattern pattern;

        public RequestURI()
        {
            pattern = BasicRules.URIReference;/*new Sequence(new Prefix("http://"),
                BasicRules.Host,
                new Optional(new Sequence(new Character(':'), BasicRules.Port)),
                new Optional(new Sequence(BasicRules.AbsPath,
                                           new Optional(new Sequence(new Character('?'), BasicRules.Query)))));*/
           // pattern = new Choice(new Character('*'), BasicRules.AbsoluteURI, BasicRules.AbsPath, BasicRules.Authority);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
