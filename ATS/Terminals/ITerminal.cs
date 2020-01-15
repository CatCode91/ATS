using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public interface ITerminal
    {
        void StartDial(int number);

        void FinishDial();

        void Connect();

        void Disconnect();
    }
}
