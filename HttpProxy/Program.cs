using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    class Program
    {
        static async Task Main()
        {
            Proxy proxy = new(8889);
            await proxy.StartAsync();
        }
    }
}