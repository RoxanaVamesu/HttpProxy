using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class ProxyClient
    {
        private readonly TcpClient browserClient;
        private readonly Stream browserClientStream;
        private readonly TcpClient proxyClient;
        private readonly Message browserClientMessage;
        private readonly Message originServerMessage;
        private int index;

        public ProxyClient(TcpClient browserClient)
        {
            this.browserClient = browserClient;
            browserClientStream = browserClient.GetStream();
            proxyClient = new TcpClient();
            browserClientMessage = new Message();
            originServerMessage = new Message();
        }

        public bool IsBadRequest { get; set; }

        public NetworkStream OriginServerStream { get; private set; }

        public async Task HandleRequestAsync()
        {
            try
            {
                await ReadRequestFromBrowserClientAsync();
                await SendRequestToOriginServerAsync();
            }
            catch (SocketException e)
            {
                if (e.Message == "No such host is known.")
                {
                    await SendNotFoud();
                    Console.WriteLine("Sent NotFound");
                    browserClient.Close();
                    proxyClient.Close();
                }
                IsBadRequest = true;
            }
            catch (IOException e)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("This is an IOException");
                Console.WriteLine($"proxyClient is connected: {proxyClient.Connected}");
                Console.WriteLine($"browserClient is connected: {browserClient.Connected}");
                Console.WriteLine($"Exception message is: {e.Message}");
                Console.WriteLine($"Sourse of the exception is: {e.Source}");
                Console.WriteLine($"Stack is: {e.StackTrace}");
                Console.WriteLine(new string('-', 50));
                browserClient.Close();
                proxyClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"proxyClient is connected: {proxyClient.Connected}");
                Console.WriteLine($"browserClient is connected: {browserClient.Connected}");
                Console.WriteLine($"Exception message is: {e.Message}");
                Console.WriteLine($"Sourse of the exception is: {e.Source}");
                Console.WriteLine($"Stack is: {e.StackTrace}");
                Console.WriteLine(new string('-', 50));
                browserClient.Close();
                proxyClient.Close();
            }
        }

        private async Task ReadRequestFromBrowserClientAsync()
        {
            do
            {
                browserClientMessage.BytesReceived = await browserClientStream.ReadAsync(browserClientMessage.Buffer.AsMemory(0, browserClientMessage.Buffer.Length));
                browserClientMessage.UpdateHeaders();
            } while (!browserClientMessage.IsHeaderEnd(out index) && browserClientMessage.BytesReceived != 0);
        }

        private async Task SendRequestToOriginServerAsync()
        {
            string browserRequestHeaders = Encoding.ASCII.GetString(browserClientMessage.GetHeadersBytes(index));

            Console.WriteLine(new string('-', 100));
            Console.WriteLine(browserRequestHeaders);
            Console.WriteLine(new string('-', 100));

            var isRequestHeaderCompliant = ValidateRequestHeader(browserRequestHeaders);
            if (isRequestHeaderCompliant)
            {
                var request = new Header(browserRequestHeaders);
                (string host, int port) = request.GetHostDetails();
                string requestHeaders = request.RequestHeaders;
                string bodyLength = request.MessageLength;

                await ConnectToOriginServerAsync(host, port);
                await SendRequestHeaderToOriginServerAsync(requestHeaders);
                await SendRequestBodyToOriginServerAsync(bodyLength);
            }
            else
            {
                await SendBadRequest();
                Console.WriteLine("Sent BadRequest");
                IsBadRequest = true;
                browserClient.Close();
                proxyClient.Close();
            }
        }

        private async Task ConnectToOriginServerAsync(string host, int port)
        {
            await proxyClient.ConnectAsync(host, port);
            OriginServerStream = proxyClient.GetStream();
        }

        private async Task SendRequestHeaderToOriginServerAsync(string requestHeaders)
        {
            byte[] bytesRequest = Encoding.UTF8.GetBytes(requestHeaders);
            await OriginServerStream.WriteAsync(bytesRequest.AsMemory(0, bytesRequest.Length));
        }

        private async Task SendRequestBodyToOriginServerAsync(string bodyLength)
        {
            byte[] bodybytes = browserClientMessage.GetBodyBytes(index);
            await OriginServerStream.WriteAsync(bodybytes.AsMemory(0, bodybytes.Length));
        }

        public async Task HandleResponseAsync()
        {
            try
            {
                await ReadResponseFromOriginServerAsync();
                await SendResponseToBrowserClientAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("This is an IOException");
                Console.WriteLine($"proxyClient is connected: {proxyClient.Connected}");
                Console.WriteLine($"browserClient is connected: {browserClient.Connected}");
                Console.WriteLine($"Exception message is: {e.Message}");
                Console.WriteLine($"Sourse of the exception is: {e.Source}");
                Console.WriteLine($"Stack is: {e.StackTrace}");
                Console.WriteLine(new string('-', 50));
            }
            catch (Exception e)
            {
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"proxyClient is connected: {proxyClient.Connected}");
                Console.WriteLine($"browserClient is connected: {browserClient.Connected}");
                Console.WriteLine($"Exception message is: {e.Message}");
                Console.WriteLine($"Sourse of the exception is: {e.Source}");
                Console.WriteLine($"Stack is: {e.StackTrace}");
                Console.WriteLine(new string('-', 50));
            }
            browserClient.Close();
            proxyClient.Close();
            Console.WriteLine(new string('+', 50));
            Console.WriteLine($"proxyClient is connected: {proxyClient.Connected}");
            Console.WriteLine($"browserClient is connected: {browserClient.Connected}");
            Console.WriteLine(new string('+', 50));
        }

        private async Task ReadResponseFromOriginServerAsync()
        {
            do
            {
                originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(originServerMessage.Buffer.AsMemory(0, originServerMessage.Buffer.Length));
                originServerMessage.UpdateHeaders();
            } while (!originServerMessage.IsHeaderEnd(out index) && originServerMessage.BytesReceived != 0);
        }

        private async Task SendResponseToBrowserClientAsync()
        {
            string serverResponseHeaders = Encoding.ASCII.GetString(originServerMessage.GetHeadersBytes(index));
            Console.WriteLine(serverResponseHeaders);
            bool isResponseHeaderCompliant = ValidateResponseHeader(serverResponseHeaders);
            if (isResponseHeaderCompliant)
            {
                var header = new Header(serverResponseHeaders);
                string bodyLength = header.MessageLength;
                await SendResponseHeaderToClientAsync();
                await SendResponseBodyToClientAsync(bodyLength);
            }
            else
            {
                await SendBadRequest();
                IsBadRequest = true;
                browserClient.Close();
                proxyClient.Close();
            }
        }

        private async Task SendResponseHeaderToClientAsync()
        {
            await browserClientStream.WriteAsync(originServerMessage.GetHeadersBytes(index).AsMemory(0, Encoding.ASCII.GetString(originServerMessage.GetHeadersBytes(index)).Length));

            Console.WriteLine("Response header was sent.");
        }

        private async Task SendResponseBodyToClientAsync(string bodyLength)
        {
            if (bodyLength == "chunked")
            {
                await SendResponseBodyByChunkAsync();
            }
            else if (bodyLength.Contains("Length"))
            {
                int contentLength = int.Parse(bodyLength.Split("=")[1]);
                await SendResponseBodyByContentLengthAsync(contentLength);
            }
            else
            {
                await SendResponseBodyByAsync();
            }
        }

        private async Task SendResponseBodyByChunkAsync()
        {
            byte[] bodyBytes = originServerMessage.GetBodyBytes(index);
            while (bodyBytes.Length > 0)
            {
                Console.WriteLine("BodyBytes = {0}", bodyBytes.Length);
                int i = Array.IndexOf(bodyBytes, (byte)13);
                int chunkSize = GetChunkSize(bodyBytes, i);
                Console.WriteLine("ChunkSize = {0}", chunkSize);
                if (chunkSize + i + 4 <= bodyBytes.Length && chunkSize != 0)
                {
                    do
                    {
                        await browserClientStream.WriteAsync(bodyBytes, 0, chunkSize + i + 4);
                        bodyBytes = bodyBytes.TakeLast(bodyBytes.Length - (chunkSize + i + 4)).ToArray();
                        Console.WriteLine("BodyBytes = {0}", bodyBytes.Length);
                        i = Array.IndexOf(bodyBytes, (byte)13);
                        chunkSize = GetChunkSize(bodyBytes, i);
                        Console.WriteLine("ChunkSize = {0}", chunkSize);
                    } while (chunkSize != 0 && chunkSize + i + 4 <= bodyBytes.Length && chunkSize != -1);
                }
                if (chunkSize == 0)
                {
                    await browserClientStream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
                    while ((originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(originServerMessage.Buffer, 0, originServerMessage.Buffer.Length)) != 0)
                    {
                        await browserClientStream.WriteAsync(originServerMessage.Buffer, 0, originServerMessage.BytesReceived);
                    }
                    Console.WriteLine("Body was sent in SendResponseBodyByChunkAsync");
                    return;
                }
                if (chunkSize != -1)
                {
                    await browserClientStream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
                    int bytesToRead = chunkSize - bodyBytes.Length + i + 4;
                    Console.WriteLine("bytesToRead = {0}", bytesToRead);
                    Array.Resize(ref bodyBytes, bytesToRead);
                    do
                    {
                        originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(bodyBytes, 0, bytesToRead);
                        Console.WriteLine("bytesReceived = {0}", originServerMessage.BytesReceived);
                        await browserClientStream.WriteAsync(bodyBytes, 0, originServerMessage.BytesReceived);
                        bytesToRead -= originServerMessage.BytesReceived;
                        Console.WriteLine("bytesToRead = {0}", bytesToRead);
                    } while (bytesToRead > 0);
                }
                originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(originServerMessage.Buffer, 0, originServerMessage.Buffer.Length);
                Array.Resize(ref bodyBytes, originServerMessage.BytesReceived);
                Array.Copy(originServerMessage.Buffer, 0, bodyBytes, 0, bodyBytes.Length);
            }
            Console.WriteLine("Body was sent in SendResponseBodyByChunkAsync");
        }

        private async Task SendResponseBodyByContentLengthAsync(int contentLength)
        {
            var body = originServerMessage.GetBodyBytes(index);
            await browserClientStream.WriteAsync(body.AsMemory(0, body.Length));
            contentLength -= body.Length;
            if (contentLength != 0)
            {
                do
                {
                    originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(originServerMessage.Buffer.AsMemory(0, originServerMessage.Buffer.Length));
                    await browserClientStream.WriteAsync(originServerMessage.Buffer.AsMemory(0, originServerMessage.BytesReceived));
                    contentLength -= originServerMessage.BytesReceived;
                } while (contentLength != 0);
            }
            Console.WriteLine("Body was sent in SendResponseBodyByContentLengthAsync");
        }

        private async Task SendResponseBodyByAsync()
        {
            await browserClientStream.WriteAsync(originServerMessage.GetBodyBytes(index).AsMemory(0, originServerMessage.GetBodyBytes(index).Length));
            while ((originServerMessage.BytesReceived = await OriginServerStream.ReadAsync(originServerMessage.Buffer.AsMemory(0, originServerMessage.Buffer.Length))) != 0)
            {
                await browserClientStream.WriteAsync(originServerMessage.Buffer.AsMemory(0, originServerMessage.BytesReceived));
            }
            Console.WriteLine("Body was sent in SendResponseBodyByAsync");
        }

        private async Task SendBadRequest()
        {
            string badRequest = Error.BadRequest;
            byte[] bytesError = Encoding.UTF8.GetBytes(badRequest);
            await browserClientStream.WriteAsync(bytesError.AsMemory(0, bytesError.Length));
        }

        private async Task SendNotFoud()
        {
            string notFound = Error.NotFound;
            byte[] bytesError = Encoding.UTF8.GetBytes(notFound);
            await browserClientStream.WriteAsync(bytesError.AsMemory(0, bytesError.Length));
        }

        private static bool ValidateRequestHeader(string messageHeader)
        {
            var validator = new Validator(messageHeader);
            return validator.CheckStartLine() && validator.CheckHeaderFields() && validator.IsHostFields();
        }

        private static bool ValidateResponseHeader(string headerMessage)
        {
            var validator = new Validator(headerMessage);
            return validator.CheckStartLine() && validator.CheckHeaderFields();
        }

        public int GetChunkSize(byte[] bodyBytes, int i)
        {
            if (i == -1)
            {
                return -1;
            }
            var chunkSizeBytes = bodyBytes.Take(i).ToArray();
            int j = Array.IndexOf(chunkSizeBytes, (byte)59);
            if (j != -1)
            {
                chunkSizeBytes = chunkSizeBytes.Take(j).ToArray();
            }
            string chunkSizeString = Encoding.UTF8.GetString(chunkSizeBytes);
            Console.WriteLine("ChunkSizeString: {0}", chunkSizeString);
            int n = int.Parse(chunkSizeString, NumberStyles.HexNumber, CultureInfo.CreateSpecificCulture("en-US"));
            return n == 0 ? 0 : n;
        }
    }
}