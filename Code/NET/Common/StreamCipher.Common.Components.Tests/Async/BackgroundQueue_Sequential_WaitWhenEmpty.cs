using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Async;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Interfaces.ActivityControl;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Components.Async;

namespace StreamCipher.Common.Components.Tests.Async
{
    class BackgroundQueue_Sequential_WaitWhenEmpty:IRunIntegrationTest
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

            _backgroundQueue.Start();

            for (int i = 0; i < 10; i++)
                _backgroundQueue.Add(i);
                        
            Console.ReadLine();
            _backgroundQueue.Shutdown();
            Logger.Info(this, "Cancel sent. Press Enter when done.");
            
            Console.ReadLine();
            Console.WriteLine(String.Format("Total Processed = {0}", totalProcessed));
            Console.ReadLine();
            
        }
    }
}
