using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Async;
using StreamCipher.Common.Components.Logging;
using System.Threading;
using StreamCipher.Common.Interfaces.ActivityControl;
using StreamCipher.Common.Components.Async;
using StreamCipher.Common.Components.Async;

namespace StreamCipher.Common.Components.Tests.Async
{
    class BackgroundQueue_Sequential_EmptyQueueBeforeShutdown:IRunIntegrationTest
    {
        private BackgroundQueue<int> _backgroundQueue;


        public void Run(string[] args)
        {
            int totalProcessed = 0;
            _backgroundQueue = new BackgroundQueue<int>((i) =>
            {
                totalProcessed++;
                Logger.Info(this, "Take operation successful: " + i.ToString());
                if (_backgroundQueue.Status != ActivationStatus.INACTIVE) Thread.Sleep(100);
            }, new BackgroundQueueConfig(true, true), new CancellationTokenSource());

            for (int i = 0; i < 10000; i++)
                _backgroundQueue.Add(i);

            _backgroundQueue.Start();

            Console.ReadLine();
            _backgroundQueue.Shutdown();
            Logger.Info(this, "Cancel sent. Press Enter when done.");
            Console.ReadLine();

            Console.WriteLine(String.Format("Total Processed = {0}", totalProcessed));
            Console.ReadLine();

        }
    }
}
