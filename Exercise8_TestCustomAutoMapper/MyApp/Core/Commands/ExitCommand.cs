
using System;

namespace MyApp.Core.Commands
{

    public class ExitCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Environment.Exit(0);

            return "";
        }
    }
}
