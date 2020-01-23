using System;

namespace ATSLibrary.Terminals
{
    public delegate void TerminalStateHandler(ITerminal sender, TerminalEventArgs e);

    public class TerminalEventArgs : EventArgs
    {
        public TerminalEventArgs(string message)
        {
            Message = message;
        }

        public string Message
        {
            get;
        }
 
    }
}
