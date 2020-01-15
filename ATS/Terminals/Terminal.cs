using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public abstract class Terminal: ITerminal
    {
        private Dogovor _dogovor;
       

        protected Terminal(Dogovor dogovor)
        {
            _dogovor = dogovor;
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

        public void Connect()
        {
            ConnectToPort(this);
        }

        public void Disconnect()
        {
            _port.DisconnectPort(this);
        }
    }
}
