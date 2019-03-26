using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Commands
{
    public interface ICommand
    {
        string Execute(string[] inputArgs);
    }
}
