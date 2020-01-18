using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal delegate void PortStateHandler(Port sender, PortEventArgs e);

    internal class PortEventArgs : EventArgs
    {
        public PortEventArgs(string message)
        {
            Message = message;
        }

        public PortEventArgs(Port outPort, Port inPort, int number)
        {
            OutPort = outPort;
            InPort = inPort;
            Number = number;
        }

        public string Message { get; private set; }
        public int Number { get; private set; }
        public Port OutPort { get; private set; }
        public Port InPort { get; private set; }
    }
}