using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irrefutable.Shared.Interfaces.Command;

namespace Irrefutable.Shared.Ioc.Tests
{
    public class DummyCommand2 : ICommand
    {

        public void Execute()
        {
            for (int i = 0; i < 1000000000; i++)
            {
                //Do nothing...
            }
        }
    }
}
