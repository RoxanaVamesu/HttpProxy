using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Response
    {
        private const string badRequestBody =
                        "<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<title>400 Bad Request</title>" +
                        "</head> " +
                        "<body>" +
                        "<h1>400 Bad Request<//h1>" +
                        "<p>The request has malformed syntax.</p>" +
                        "<hr>" +
                        "<address>Roxana-Larisa HTTP Server v0.1</address>" +
                        "</body>" +
                        "</html>";

        private const string badGatewayBody = "";

        private readonly string statusLine;
        private readonly string responseHeaders;
        private readonly string responseBody;

        public Response(string startLine, string headers, string body, int bodyLength)
        {
            statusLine = startLine;
            responseHeaders = headers;
            responseBody = body;
            BodyLength = bodyLength;
        }

        public string StatusLine
        {
            get
            {
                return statusLine switch
                {
                    "400 Bad Request" => "HTTP/1.1 400 Bad Request\r\n",
                    "502 Bad Gateway" => "HTTP/1.1 502 Bad Gateway\r\n",
                    _ => statusLine
                };
            }
        }

        public string ResponseHeaders => responseHeaders;

        public string ResponseBody
        {
            get
            {
                return statusLine switch
                {
                    "400 Bad Request" => badRequestBody,
                    "502 Bad Gateway" => badGatewayBody,
                    _ => responseBody
                };
            }
        }

        public int BodyLength { get; }
    }
}