using StreamCipher.Common.Interfaces.Command;

namespace StreamCipher.Common.Tests.Ioc.Interception
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
