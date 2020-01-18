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

        /// <summary>
        /// Поднята ли трубка
        /// </summary>
        private bool _isTubeUp = false;

        public event TerminalStateHandler Ringing;

        protected Terminal(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool IsTubeUp => _isTubeUp;

        /// <summary>
        /// Подключить терминал к порту
        /// </summary>
        /// <param name="port"></param>
        public void ConnectPort(Port port)
        {
            if (_port != null)
            {
                Console.WriteLine($"Ваше устройство уже подкючено к порту {_port.PortNumber}");
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
                Console.WriteLine($"Порт для устройства не найден!");
                return;
            }

            _port.DisconnectTerminal(this, ref _port);
        }

        /// <summary>
        /// Позвонить по номеру
        /// </summary>
        /// <param name="number"></param>
        public void StartDial(int number)
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
            _isTubeUp = false;
        }

        /// <summary>
        /// Ответить на входящий вызов
        /// </summary>
        public void AcceptCall()
        {
            _port.AcceptCall();
        }

        /// <summary>
        /// Сбросить вызов
        /// </summary>
        public void SendBusy()
        {
            _port.BusySent();
        }

        /// <summary>
        /// Входящий вызов, метод подписаный к событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _port_RingNotify(Port sender, PortEventArgs e)
        {
            Ringing?.Invoke(this, new TerminalEventArgs(e.Message));
        }

    }
}
