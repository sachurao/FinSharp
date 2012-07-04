﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamCipher.FinSharp.WorkBench.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace StreamCipher.FinSharp.BondAnalysisModule
{
    [Module(ModuleName = "Bond Analysis Tools")]
    public class ModuleDefinition:IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleDefinition(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }


        #region Implementation of IModule

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(DesktopDefaults.WindowRegionWorkspace, typeof(BondCalculator));
        }

        #endregion
    }
}
