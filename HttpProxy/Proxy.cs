using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Proxy
    {
        private readonly TcpListener tcpListener;

        public Proxy(int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        }

        public async Task StartAsync()
        {
            tcpListener.Start();
            Console.WriteLine("The proxy server is running and waiting for clients!");
            while (true)
            {
                var browserClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("Client is successfully connected!");
                _ = HandleClientAsync(browserClient);
            }
        }

        private static async Task HandleClientAsync(TcpClient browserClient)
        {
            var client = new ProxyClient(browserClient);
            await client.HandleRequestAsync();
            if (!client.IsBadRequest)
            {
                await client.HandleResponseAsync();
            }
        }
    }
}