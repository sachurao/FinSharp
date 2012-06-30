using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Components.Async;
using Irrefutable.Shared.Components.Logging;
using Irrefutable.Shared.Interfaces.ActivityControl;
using System.Threading;

namespace Irrefutable.Shared.Components.Tests.Async
{
    class BackgroundQueue_AddingWhenStopped_ThrowsException:IRunIntegrationTest
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
            });

            _backgroundQueue.Start();

            for (int i = 0; i < 10; i++)
                _backgroundQueue.Add(i);

            Console.ReadLine();
            _backgroundQueue.Shutdown();
            Logger.Info(this, "Cancel sent. Press Enter when done.");

            Console.ReadLine();
            try
            {
                _backgroundQueue.Add(11);
            }
            catch (Exception ex)
            {
                Logger.Error(this, "Received exception as expected.", ex);
            }
            Console.WriteLine(String.Format("Total Processed = {0}", totalProcessed));
            Console.ReadLine();

        }
    }
}
