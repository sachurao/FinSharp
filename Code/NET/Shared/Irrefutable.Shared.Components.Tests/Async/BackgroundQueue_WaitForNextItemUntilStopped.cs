using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Components.Async;
using System.Threading;
using Irrefutable.Shared.Components.Logging;

namespace Irrefutable.Shared.Components.Tests.Async
{
    class BackgroundQueue_WaitForNextItemUntilStopped:IRunIntegrationTest
    {
        private BackgroundQueue<int> _backgroundQueue;
        private CancellationTokenSource _src;

        public BackgroundQueue_WaitForNextItemUntilStopped()
        {
            _src = new CancellationTokenSource();
        }

        public void Run(string[] args)
        {
            int processed = 0;
            _backgroundQueue = new BackgroundQueue<int>((i) =>
            {
                processed++;
                Logger.Info(this, "Take operation successful: " + i.ToString());
            });

            _backgroundQueue.Start();

            ThreadPool.QueueUserWorkItem((state) =>
            {
                int i = 0;
                while (i < 1000)
                {
                    Logger.Info(this, "Trying to add item: " + i.ToString());
                    _backgroundQueue.Add(i++);
                }
                Console.WriteLine("Please Enter to add next set.");
                Console.ReadLine();
                while (i < 2000)
                {
                    Logger.Info(this, "Trying to add item: " + i.ToString());
                    _backgroundQueue.Add(i++);
                }
                
            });

            while (processed <= 1999)
            {
            }

            Console.WriteLine("Sending cancel.  Background queue should shutdown now. Press Enter when done.");
            _backgroundQueue.Shutdown();
            Console.ReadLine();
        }
    }
}
