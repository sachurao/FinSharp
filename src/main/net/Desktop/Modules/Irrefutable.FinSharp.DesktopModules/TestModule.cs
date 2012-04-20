using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.FinSharp.Desktop.Common;
using Irrefutable.FinSharp.DesktopModules.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace Irrefutable.FinSharp.DesktopModules
{
    public class TestModule:IModule
    {
        private readonly IRegionManager _regionManager;

        public TestModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region Implementation of IModule

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(DesktopDefaults.DESKTOP_REGION_MAIN, typeof (TestView));
        }

        #endregion
    }
}
