using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlowlorisSharp.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            var slow = new Slowloris("Target", cts.Token);
            var slowThread = new Thread(slow.Manage);
            slowThread.Start();

            //if (!slow.CancelToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        cts.Cancel();
            //    }
            //    catch { }
            //}
        }
    }
}
