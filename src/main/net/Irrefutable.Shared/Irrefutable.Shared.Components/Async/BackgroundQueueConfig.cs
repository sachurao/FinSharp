using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Irrefutable.Shared.Components.Async
{
    public class BackgroundQueueConfig
    {
        public BackgroundQueueConfig()
        {
            ProcessSynchronously = true;
            ProcessAllItemsBeforeShutdown = false;
            BoundedCapacity = Int32.MaxValue;
        }

        public bool ProcessSynchronously { get; set; }
        public bool ProcessAllItemsBeforeShutdown { get; set; }
        public Int32 BoundedCapacity { get; set; }
    }


}
