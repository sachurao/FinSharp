using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.Common.Logging;

namespace StreamCipher.Common.Async
{
    /// <summary>
    /// Configurable implementation of the producer-consumer pattern.
    /// Supports co-operative cancellation.
    /// </summary>
    /// <typeparam name="T">Item to be queued.</typeparam>
    public class BackgroundQueue<T>:ControlledComponent
    {
        //TODO: DefaultBackgroundQueue should be disposable.

        #region Member Variables

        
        private readonly BlockingCollection<T> _blockingQueue;
        private readonly Task _processor;
        private readonly BackgroundQueueConfig _config;
        private readonly CancellationTokenSource _cancellationSource;

        private static int _instanceNum = 0;
        
        #endregion

        #region Init

        public BackgroundQueue(Action<T> processItem)
            : this(processItem, new BackgroundQueueConfig.Builder().Build(), new CancellationTokenSource())
        {
        }

        public BackgroundQueue(Action<T> processItem,
                               BackgroundQueueConfig config,
                               CancellationTokenSource cancellationSource) 
            :base("Background Queue", _instanceNum++)
        {
            _config = config;
            _cancellationSource = cancellationSource;
            
            //By default it uses a ConcurrentQueue as its underlying datastore.
            _blockingQueue = new BlockingCollection<T>(config.BoundedCapacity);

            //Creating a task that handles any new item added to the queue
            _processor = new Task(() =>
            {
                Func<bool> checkIfStoppedOrFinished =
                    () => _config.ProcessAllItemsBeforeShutdown
                            ? _blockingQueue.IsCompleted
                            : _blockingQueue.IsAddingCompleted;

                while (!checkIfStoppedOrFinished())
                {
                    //Has not been marked complete for adding and is not empty
                    try
                    {
                        //Will wait indefinitely for the next item, unless cancelled
                        T item = _blockingQueue.Take(_cancellationSource.Token);

                        if (_config.ProcessSynchronously) processItem(item);
                        else
                        {
                            //Pawning off the actual work to another task, so consumer can continue...
                            Task.Factory.StartNew(i => processItem(item),
                                                TaskCreationOptions.AttachedToParent);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Logger.Info(this,
                                    "Received an OperationCanceledException when "+
                                    "trying to take next item from BlockingCollection.");
                    }
                    catch (InvalidOperationException)
                    {
                        Logger.Info(this,
                                    "Received an InvalidOperationException because the "+
                                    "BlockingCollection has been marked as complete.");
                    }
                }
                Logger.Info(this, "This BlockingCollection has been stopped.");
            }, _cancellationSource.Token);
        }

        #endregion

        #region Public Methods

        public void Add(T item)
        {
            Logger.Debug(this, "Adding item.");
            _blockingQueue.Add(item, _cancellationSource.Token);
            Logger.Debug(this, "Completed adding item.");
        }

        public void Add(IEnumerable<T> items)
        {
            Logger.Debug(this, "Adding items.");
            foreach (var item in items)
            {
                _blockingQueue.Add(item, _cancellationSource.Token);
            }
            Logger.Debug(this, "Completed adding items.");
            
        }

        public int TotalItems { get { return _blockingQueue.Count; } } 
        
        #endregion

        #region ControlledComponent

        protected override void StartCore()
        {
            _processor.Start();
        }

        protected override void ShutdownCore()
        {
            _blockingQueue.CompleteAdding();
            if (!_config.ProcessAllItemsBeforeShutdown) _cancellationSource.Cancel();
        }

        #endregion


        
    }
}
