using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public delegate void TerminalStateHandler(object sender, TerminalEventArgs e);

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
