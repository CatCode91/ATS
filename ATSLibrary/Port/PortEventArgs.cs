﻿using System;

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

        public string Message
        {
            get;
            private set;
        }

        public int AbonentNumber
        {
            get;
            private set;
        }  
    }
}