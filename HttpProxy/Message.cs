using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpProxy
{
    public class Message
    {
        private byte[] headersBytes;
        private int bytesReceived;

        public Message()
        {
            headersBytes = Array.Empty<byte>();
        }

        public byte[] Headers => headersBytes;

        public int BytesReceived
        {
            get => bytesReceived;
            set => bytesReceived = value;
        }

        public byte[] Buffer = new byte[100000];

        public void UpdateHeaders()
        {
            Array.Resize(ref headersBytes, headersBytes.Length + BytesReceived);
            Array.Copy(Buffer, 0, headersBytes, headersBytes.Length - BytesReceived, BytesReceived);
        }

        public byte[] GetBodyBytes(int index)
        {
            byte[] body = new byte[Headers.Length - index - 4];
            Array.Copy(headersBytes, index + 4, body, 0, headersBytes.Length - index - 4);
            return body;
        }

        public byte[] GetHeadersBytes(int index)
        {
            return headersBytes.SkipLast(Headers.Length - index - 4).ToArray();
        }

        public bool IsHeaderEnd(out int index)
        {
            IEnumerable<byte> sequence = new byte[] { 13, 10, 13, 10 };
            for (int i = 0; i <= Headers.Length - 4; i++)
            {
                IEnumerable<byte> substring = Headers.Skip(i).Take(4);
                if (sequence.SequenceEqual(substring))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public int GetRemainingBytesFromBody(int index)
        {
            if (GetBodyBytes(index).Length + GetHeadersBytes(index).Length == BytesReceived)
            {
                return GetBodyBytes(index).Length;
            }
            return BytesReceived;
        }
    }
}