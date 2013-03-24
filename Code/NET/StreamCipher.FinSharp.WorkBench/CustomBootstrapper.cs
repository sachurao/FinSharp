using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;

namespace StreamCipher.FinSharp.WorkBench
{
    /// <summary>
    /// This is the class that hooks up the entire application when it is started up.
    /// For simplicity's sake, using UnityBootstrapper, but using own version of logger
    /// and loading modules from sub-directory. 
    /// </summary>
    class CustomBootstrapper : UnityBootstrapper
    {
        //TODO:  Override CreateLogger() method to return your own instance of ILoggerFacade

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();

        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            //if (!Directory.Exists(DesktopDefaults.RELATIVE_PATH_MODULES))
            //    Directory.CreateDirectory(DesktopDefaults.RELATIVE_PATH_MODULES);
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = Environment.CurrentDirectory };

        }
    }

}
