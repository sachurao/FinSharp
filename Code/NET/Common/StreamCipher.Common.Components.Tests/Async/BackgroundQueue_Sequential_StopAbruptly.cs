using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.Common.Components.Async;
using System.Threading;
using StreamCipher.Common.Components.Logging;
using StreamCipher.Common.Components.Async;

namespace StreamCipher.Common.Components.Tests.Async
{
    class BackgroundQueue_Sequential_StopAbruptly:IRunIntegrationTest
    {
        private BackgroundQueue<int> _backgroundQueue;
                    
        
        public void Run(string[] args)
        {
            _backgroundQueue = new BackgroundQueue<int>((i) =>
            {
                Logger.Info(this, "Take operation successful: " + i.ToString());
                Thread.Sleep(100);
            });

            for (int i = 0; i < 10000; i++)
                _backgroundQueue.Add(i);
            
            _backgroundQueue.Start();

            Console.ReadLine();
            _backgroundQueue.Shutdown();
            Logger.Info(this, "Cancel sent. Press Enter when done.");
            Console.ReadLine();
        }

            
    }
}
