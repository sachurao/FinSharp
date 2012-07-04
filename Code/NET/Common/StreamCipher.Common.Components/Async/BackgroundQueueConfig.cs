using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StreamCipher.Common.Components.Async
{
    /// <summary>
    /// Immutable class that contains config for <see>
    ///                                            <cref>BackgroundQueue</cref>
    ///                                          </see>
    /// </summary>
    public class BackgroundQueueConfig
    {
        public BackgroundQueueConfig() : this(true, false)
        {
            
        }
        

        public BackgroundQueueConfig(bool processSynchronously, bool processAllBeforeShutdown, int boundedCapacity = Int32.MaxValue)
        {
            ProcessSynchronously = processSynchronously;
            ProcessAllItemsBeforeShutdown = processAllBeforeShutdown;
            BoundedCapacity = boundedCapacity;
        }

        public bool ProcessSynchronously { get; private set; }
        public bool ProcessAllItemsBeforeShutdown { get; private set; }
        public Int32 BoundedCapacity { get; private set; }
    }


}
