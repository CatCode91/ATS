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

        protected Terminal(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void StartDial(int number)
        {

        }

        public void FinishDial()
        {
        
        }

        public void Connect(Port port)
        {
            Console.WriteLine($"Запрос подключения от {Name}");
            _port = port;
            _port.ConnectTerminal(this);          
        }

        public void Disconnect()
        {
            Console.WriteLine($"Запрос отключения от {Name}");

            if (_port == null)
            {
                Console.WriteLine($"Порт для устройства не найден!");
                return;
            }

            _port.DisconnectTerminal(this,ref _port);
            _port = null;
        }
    }
}
