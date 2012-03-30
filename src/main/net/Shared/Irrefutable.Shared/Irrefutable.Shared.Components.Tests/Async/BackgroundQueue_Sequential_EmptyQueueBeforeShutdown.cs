using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Utilities.Async;
using Irrefutable.Shared.Components.Logging;
using System.Threading;
using Irrefutable.Shared.Interfaces.ActivityControl;
using Irrefutable.Shared.Components.Async;

namespace Irrefutable.Shared.Components.Tests.Async
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
            }, new BackgroundQueueConfig()
            {
                ProcessAllItemsBeforeShutdown = true,
                ProcessSynchronously = true
            });

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
