using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Method : IPattern
    {
        private readonly string method;

        public Method()
        {
            method = "OPTIONS,GET,HEAD,POST,PUT,DELETE,TRACE,CONNECT";
        }

        public IMatch Match(string text)
        {
            var meth = "";
            for (int i = 0; i < text.Length - 1; i++)
            {
                meth += text[i];
                if(method.Split(',').Any(m=> meth.Equals(m)))
                {
                    return new Match(true, text[(i + 1)..]);
                }
            }

            return new Match(false, text);
        }
    }
}
