using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Terminals
{
    public class Terminal : ITerminal
    {
        private Port _port;

        public event TerminalStateHandler Ringing;

        internal Terminal(string model)
        {
            Name = model.ToString();
        }

        public string Name { get; }

        /// <summary>
        /// Подключить терминал к порту
        /// </summary>
        /// <param name="port"></param>
        public void ConnectPort(Port port)
        {
            if (_port != null)
            {
                Console.WriteLine($"Ваше устройство уже подкючено!");
                return;
            }

            Console.WriteLine($"Запрос подключения от {Name}");
            _port = port;
            _port.ConnectTerminal(this);
            _port.RingNotify += _port_RingNotify;
        }

        /// <summary>
        /// Отключить терминал от порта
        /// </summary>
        public void DisconnectPort()
        {
            Console.WriteLine($"Запрос отключения от {Name}");

            if (_port == null)
            {
                Console.WriteLine($"Устройство не подключено к порту!");
                return;
            }

            _port.DisconnectTerminal(this);
        }

        /// <summary>
        /// Позвонить по номеру
        /// </summary>
        /// <param name="number"></param>
        public void Dial(int number)
        {
            if (_port == null)
            {
                Console.WriteLine("Устройство не подключено к порту!");
                return;
            }

            _port.OutcomeCalling(number);
        }

        /// <summary>
        /// Закончить разговор
        /// </summary>
        public void FinishDial()
        {
            _port.FinishTalking();
        }

        /// <summary>
        /// Подтвердить/отклонить входящий вызов
        /// </summary>
        /// <param name="accepted">True - подвердить, False - отклонить</param>
        public void SendAcceptCall(bool accepted)
        {
            _port.SendAccept(accepted);
        }

        /// <summary>
        /// Входящий вызов, метод подписаный к событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _port_RingNotify(Port sender, int number)
        {
            Ringing?.Invoke(this, new TerminalEventArgs($"Входящий вызов от абонента {number}"));
        }
    }
}
