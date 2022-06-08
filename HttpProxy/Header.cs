using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Header
    {
        private readonly string[] headerFields;

        public Header(string headerFields)
        {
            this.headerFields = headerFields.Split("\r\n");
        }

        public string MessageLength
        {
            get
            {
                return HeaderFields.ContainsKey("Transfer-Encoding") ? HeaderFields["Transfer-Encoding"]
                     : HeaderFields.ContainsKey("Content-Length") ? "Length =" + HeaderFields["Content-Length"] : "";
            }
        }

        public string StartLine => headerFields[0];

        public string Method => StartLine.Split(' ')[0];

        public Dictionary<string, string> HeaderFields
        {
            get
            {
                return GetHeaderFields();
            }
        }

        public string RequestHeaders
        {
            get
            {
                return RebuildRequest();
            }
        }

        public string Host
        {
            get
            {
                return HeaderFields["Host"];
            }
        }

        public (string host, int port) GetHostDetails()
        {
            int portIndex = Host.IndexOf(':');
            return (portIndex == -1 ? Host : Host[..portIndex], portIndex == -1 ? 80 : Int32.Parse(Host[(portIndex + 1)..]));
        }

        private string RebuildRequest()
        {
            string result = "";
            for (int i = 0; i < headerFields.Length - 1; i++)
            {
                result += headerFields[i] + "\r\n";
            }
            return result;
        }

        private Dictionary<string, string> GetHeaderFields()
        {
            Dictionary<string, string> headerFields = new();
            for (int i = 1; i < this.headerFields.Length; i++)
            {
                int index = this.headerFields[i].IndexOf(": ");
                if (index > 0)
                {
                    string key = this.headerFields[i].Substring(0, index);
                    string value = this.headerFields[i][(index + 2)..].Trim();
                    if (!headerFields.TryAdd(key, value))
                    {
                        headerFields[key] += "," + value;
                    }
                }
            }
            return headerFields;
        }
    }
}