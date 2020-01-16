using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Terminals
{
    public interface ITerminal
    {
        void StartDial(int number);

        void FinishDial();

        void Connect(Port port);

        void Disconnect();
    }
}
