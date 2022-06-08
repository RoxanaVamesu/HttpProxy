using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public static class Error
    {
        public static string BadRequest=> "HTTP/1.1 400 Bad Request\r\n\r\n" +
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

        public static string NotFound=> "HTTP/1.1 404 Not Found\r\n\r\n"+
                        "<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<title>404 Not Found</title>" +
                        "</head> " +
                        "<body>" +
                        "<h1>404 Not Found<//h1>" +
                        "<p>The requested resource could not be found but may be available in the future.</p>" +
                        "<hr>" +
                        "<address>Roxana-Larisa HTTP Server v0.1</address>" +
                        "</body>" +
                        "</html>";
    }
}
