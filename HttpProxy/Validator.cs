using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Validator
    {
        private readonly string message;

        public Validator(string headersMessage)
        {
            message = headersMessage;
        }

        public bool CheckHeaderFields()
        {
            return GetHeadersFields().All(f => f.Contains(':')&& CheckMessageHeader(f));
        }

        public bool IsHostFields()
        {
            return GetHeadersFields().Any(f => f.Contains("Host"));
        }

        public bool CheckStartLine()
        {
            return new StartLine().Match(GetStartLine()).Success;
        }

        private bool CheckMessageHeader(string field)
        {
            var elements = field.Split(':');
            return new Token().Match(elements[0]).Success && new Value().Match(elements[1]).Success;
        }

        private List<string> GetHeadersFields()
        {
            return Enumerable.Range(1, HeaderLines().Length - 4).Aggregate(new List<string>(), (a, i) => { a.Add(HeaderLines()[i]); return a; });
        }

        private string[] HeaderLines()
        {
            return message.Split("\r\n");
        }

        private string GetStartLine()
        {
            return HeaderLines()[0];
        }

    }
}
