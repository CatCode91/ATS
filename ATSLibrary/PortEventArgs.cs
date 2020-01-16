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
        public PortEventArgs(PortStatus status)
        {
           Status = status;
        }

        public PortStatus Status { get; private set; }
    }
}