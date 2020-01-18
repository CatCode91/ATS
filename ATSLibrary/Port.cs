using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ATSLibrary
{
    public class Port
    {
        private int _portNumber;
        //абоненский номер на порту
        private PortStatus _status;
        private int _abonentNumber;
        //терминал, подключенный к порту
        private ITerminal _terminal;
        //текущий звонок на порту
        private Call currentCall;
        //история звонков
        private List<Call> callsHistory;
        //принят ли звонок другим абонентом
        private bool isCallAccepted = false;

        internal event PortStateHandler PortConnected;
        internal event PortStateHandler PortDisconnected;
        internal event PortStateHandler OutcomeCall;
        internal event PortStateHandler CallAccepted;
        internal event PortStateHandler RingNotify;

        internal Port()
        {
            _status = PortStatus.Free;
        }


        public int AbonentNumber => _abonentNumber;
        public PortStatus Status => _status;
        internal int PortNumber => _portNumber;

        /// <summary>
        /// Подключить терминал к порту
        /// </summary>
        /// <param name="terminal"></param>
        internal void ConnectTerminal(ITerminal terminal)
        {
            if (_terminal == null)
            {
                _terminal = terminal;
                PortConnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Устройство подключено к порту!"));
            }

            else
            {
                Console.WriteLine($"Port №{PortNumber}:  Порт уже используется!");
            }

           
        }      
      
        /// <summary>
        /// Отключить терминал от порта
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="port"></param>
        internal void DisconnectTerminal(ITerminal terminal,ref Port port)
        {
            if (_terminal == null)
            {
                Console.WriteLine("Чтобы что-то отключить, для начала это нужно подключить :)");
                return;
            }

            if (terminal != _terminal)
            {
                Console.WriteLine($"Port №{PortNumber}:  Порт занят, отключение с вашего устройства не возможно!");
                return;
            }

            _terminal = null;
            port = null;
            PortDisconnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Терминал отключен от порта!"));
        }

        /// <summary>
        /// На порт поступает входящий вызов
        /// </summary>
        /// <param name="incomeNumber"></param>
        internal void IncomeCalling(int incomeNumber)
        {
            RingNotify?.Invoke(this, new PortEventArgs($"Входящий вызов от абонента {incomeNumber}"));
        }
        
        /// <summary>
        /// Исходящий вызов
        /// </summary>
        /// <param name="number"></param>
        internal void OutcomeCalling(int number)
        {
            if (AbonentNumber == number)
            {
                Console.WriteLine("Нельзя позвонить самому себе!");
                return;
            }

            _status = PortStatus.Busy;

            //вызываем событие, передавая номер вызываемого абонента
            OutcomeCall?.Invoke(this, new PortEventArgs(this,null,number));

            if (!isCallAccepted)
            {
                Console.WriteLine("Вызываемый абонент cбросил вызов!");
                return;
            }

            DateTime timeStart = DateTime.Now;

            Talking();
   
            DateTime timeFinish = DateTime.Now;

            TimeSpan duriation = timeFinish - timeStart;           
        }

        /// <summary>
        /// "Разговор" между двумя портами
        /// </summary>
        internal void Talking()
        {
            if (_terminal.IsTubeUp)
            {
                Console.WriteLine("идет разговор....");
                Thread.Sleep(1000);
                Talking();
            }
        }

        /// <summary>
        /// "Сбросить" вызов
        /// </summary> 
        internal void BusySent()
        {
            isCallAccepted = false;
            _status = PortStatus.Connected;
            Port port = this;
            Console.WriteLine("Вызов сброшен!");   
        }

        /// <summary>
        /// Закончить разговор
        /// </summary>
        internal void FinishTalking(Call call)
        {
            callsHistory.Add(call);
            _status = PortStatus.Connected;
        }

        /// <summary>
        /// Присваивание порту собственного и абонентского номеров
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="abonentNumber"></param>
        internal void SetAbonentNumber(int portNumber,int abonentNumber)
        {
            _portNumber = portNumber;
            _abonentNumber = abonentNumber;
            _status = PortStatus.Disconnected;  
        }

        /// <summary>
        /// Подвердить входящий вызов
        /// </summary>
        internal void AcceptCall()
        {
            CallAccepted?.Invoke(this, new PortEventArgs(this.AbonentNumber));
        }

        /// <summary>
        /// Изменяет статус порта
        /// </summary>
        /// <param name="status"></param>
        internal void PortStatusChange(PortStatus status)
        {
            _status = status;
        }
    }
}
