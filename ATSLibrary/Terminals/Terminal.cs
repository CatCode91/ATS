using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Terminals
{
    public abstract class Terminal: ITerminal
    {
        private Port _port;

        public event TerminalStateHandler Ringing;

        protected Terminal(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void ConnectPort(Port port)
        {
            Console.WriteLine($"Запрос подключения от {Name}");
            _port = port;
            _port.ConnectTerminal(this);
            _port.RingNotify += _port_RingNotify;
        }

        public void DisconnectPort()
        {
            Console.WriteLine($"Запрос отключения от {Name}");

            if (_port == null)
            {
                Console.WriteLine($"Порт для устройства не найден!");
                return;
            }

            _port.DisconnectTerminal(this, ref _port);
            _port = null;
        }

        public void StartDial(int number)
        {
            _port.OutcomeCalling(number);
        }

        public void FinishDial()
        {
        
        }

        private void _port_RingNotify(Port sender, PortEventArgs e)
        {
            Ringing?.Invoke(this,new TerminalEventArgs(e.Message));
        }

        public void AnswerCall()
        {
            _port.Talking();
        }

        public void SendBusy()
        {
            
        }
    }
}
