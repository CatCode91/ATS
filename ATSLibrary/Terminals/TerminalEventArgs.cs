using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Terminals
{
    public delegate void TerminalStateHandler(ITerminal sender, TerminalEventArgs e);

    public class TerminalEventArgs
    {
        public TerminalEventArgs(string message)
        {
            message = Message;
        }

        public string Message
        {
            get;
        }
    }
}
