using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamCipher.FinSharp.Services.Functions;

namespace StreamCipher.FinSharp.Services
{
    public partial class GenericService : ServiceBase
    {
        public GenericService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Always start on a background thread...
            BondAnalysis bondSvc = new BondAnalysis();
            ThreadPool.QueueUserWorkItem(state => bondSvc.Start());

        }

        protected override void OnStop()
        {
        }
    }
}
