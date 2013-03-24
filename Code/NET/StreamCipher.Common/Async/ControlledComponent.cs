using System;
using System.Threading;
using StreamCipher.Common.Interfaces.ActivityControl;
using StreamCipher.Common.Utilities.Concurrency;

namespace StreamCipher.Common.Async
{

    /// <summary>
    /// All controlled activities should inherit from this object.
    /// </summary>
    public class ControlledComponent : IControlledComponent
    {
        #region Member Variables

        private AtomicBoolean _isInitialised = new AtomicBoolean(false);
        private ActivationStatus _status = ActivationStatus.INACTIVE;
        private String _componentName;

        #endregion

        #region Init

        public ControlledComponent(String name) : this(name, 0)
        {
        }

        public ControlledComponent(String name, int instanceNum)
        {
            _componentName = String.Format("{0} - Instance {1}", name, instanceNum);
        }

        #endregion

        #region Implementation of IControlledComponent

        public void Start()
        {
            if (_isInitialised.CompareAndSet(false, true))
            {
                Logging.Logger.Debug(this, String.Format("Starting component {0}", _componentName));
                StartCore();
                _status = ActivationStatus.ACTIVE;
                Logging.Logger.Debug(this, String.Format("Started component {0}", _componentName));
            }

        }

        public void Shutdown()
        {
            if (_isInitialised.CompareAndSet(true, false))
            {
                Logging.Logger.Debug(this, String.Format("Shutting down component {0}", _componentName));
                ShutdownCore();
                _status = ActivationStatus.INACTIVE;
                Logging.Logger.Debug(this, String.Format("Shut down component {0}", _componentName));
            }

        }

        public ActivationStatus Status
        {
            get { return _status; }
        }

        #endregion
        
        #region Protected Methods

        protected virtual void StartCore()
        {
            //Do nothing...
        }

        protected virtual void ShutdownCore()
        {
            //Do nothing...
        }

        #endregion


    }
}
