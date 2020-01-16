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

        public PortEventArgs(PortStatus status)
        {
           Status = status;
        }

        public PortEventArgs(int dialNumber, PortStatus status)
        {
            dialNumber = DialNumber;
            Status = status;
        }

        public string Message { get; private set; }
        public int DialNumber { get; private set; }
        public PortStatus Status { get; private set; }


    }
}