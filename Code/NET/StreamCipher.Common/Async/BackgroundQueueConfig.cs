using System;

namespace StreamCipher.Common.Async
{
    /// <summary>
    /// Immutable class that contains config for <see>
    ///                                            <cref>BackgroundQueue</cref>
    ///                                          </see>
    /// </summary>
    public class BackgroundQueueConfig
    {
        #region Init

        private BackgroundQueueConfig()
        {

        }

        public class Builder
        {
            private bool _processSync = true;
            private bool _processAllBeforeShutdown = false;
            private Int32 _capacity = Int32.MaxValue;

            public bool ProcessSync
            {
                set { _processSync = value; }
            }

            public bool ProcessAllBeforeShutdown
            {
                set { _processAllBeforeShutdown = value; }
            }

            public int Capacity
            {
                set { _capacity = value; }
            }

            public BackgroundQueueConfig Build()
            {
                var retVal = new BackgroundQueueConfig()
                    {
                        BoundedCapacity = _capacity,
                        ProcessAllItemsBeforeShutdown = _processAllBeforeShutdown,
                        ProcessSynchronously = _processSync
                    };
                return retVal;
            }
        }

        #endregion

        #region Public Properties

        public bool ProcessSynchronously { get; private set; }
        public bool ProcessAllItemsBeforeShutdown { get; private set; }
        public Int32 BoundedCapacity { get; private set; }

        #endregion

    }


}
