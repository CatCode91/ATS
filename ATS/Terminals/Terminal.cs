using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public abstract class Terminal: ITerminal
    {
        private Port _port;

        protected Terminal(Dogovor dogovor)
        {
            _port = new Port(dogovor);
        }

        public void StartDial(int number)
        {
            if (_port.Status == PortStatus.Disconnected)
            {
                Console.WriteLine("Сначала подключите терминал к порту");
                return;
            }

            _port.Calling(number);
        }

        public void FinishDial()
        {
            _port.FinishTalking(new Call());
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
