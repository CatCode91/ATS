using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public abstract class Terminal: ITerminal
    {
        private int _number;
        private Port _port;

        protected Terminal(Dogovor dogovor)
        {
            _port = new Port(dogovor.DogovorNumber);
        }

        public void StartDial(int number)
        {
            if (_port.Status == PortStatus.Disconnected)
            {
                return;
            }

            _port.Calling(number);
        }

        public void FinishDial()
        {
            _port.FinishTalking();
        }

        public void ConnectPort()
        {
            _port.Connect(this);
        }

        public void DisconnectPort()
        {
            _port.Disconnect(this);
        }
    }
}
