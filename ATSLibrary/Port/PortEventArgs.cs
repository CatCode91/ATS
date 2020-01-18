using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public delegate void PortStateHandler(Port sender, PortEventArgs e);

    public class PortEventArgs : EventArgs
    {
        public PortEventArgs(string message)
        {
            Message = message;
        }

        public PortEventArgs(int abonentNumber)
        {
            AbonentNumber = abonentNumber;
        }

        public PortEventArgs(bool isCallAccepted)
        {
            IsCallAccepted = isCallAccepted;
        }

        public string Message { get; private set; }
        public int AbonentNumber { get; private set; }
        public bool IsCallAccepted { get; private set; }
    }
}