﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using StreamCipher.FinSharp.WorkBench.Common;

namespace StreamCipher.FinSharp.WorkBench
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new CustomBootstrapper();
            bootstrapper.Run();
        }
    }
}