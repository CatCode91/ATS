using System;
using System.Collections.Generic;
using System.Threading;

namespace ATSLibrary
{
    public class Port
    {
        public   event PortStateHandler RingNotify;
        internal event PortStateHandler PortConnected;
        internal event PortStateHandler PortDisconnected;
        internal event PortStateHandler OutcomeCall;
        internal event PortStateHandler CallFinish;

        internal Port()
        {
            Status = PortStatus.Free;
        }

        public int AbonentNumber { get; private set; }
        public PortStatus Status { get; private set; }

        internal bool IsCallAccepted { get; private set; }
        internal bool StopCall { get; private set; } = false;
        internal int PortNumber { get; private set; }
        internal int DogovorNumber { get; private set; }



        /// <summary>
        /// Подключить терминал к порту
        /// </summary>
        /// <param name="terminal"></param>
        internal void ConnectTerminal(ITerminal terminal)
        {
                PortConnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Устройство подключено к порту!"));
        }      
      
        /// <summary>
        /// Отключить терминал от порта
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="port"></param>
        internal void DisconnectTerminal(ITerminal terminal)
        {
            PortDisconnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Терминал отключен от порта!"));
        }

        /// <summary>
        /// На порт поступает входящий вызов
        /// </summary>
        /// <param name="incomeNumber"></param>
        internal void IncomeCalling(Port port)
        {
            //оповестить о входящем вызове подписанный терминал
            RingNotify?.Invoke(this, new PortEventArgs(port.AbonentNumber));
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

            //вызываем событие, передавая номер вызываемого абонента
            OutcomeCall?.Invoke(this, new PortEventArgs(number));
        }

        /// <summary>
        /// Подвердить входящий вызов
        /// </summary>
        internal void SendAccept(bool accepted)
        {
            IsCallAccepted = accepted;
        }

        /// <summary>
        /// Закончить разговор
        /// </summary>
        internal void FinishTalking()
        {
            CallFinish?.Invoke(this, new PortEventArgs(null));
        }

        /// <summary>
        /// Изменяет статус порта
        /// </summary>
        /// <param name="status"></param>
        internal void PortStatusChange(PortStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Инициализация свойств собственного и абонентского номеров порта
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="abonentNumber"></param>
        internal void SetAbonentNumber(int dogovorNumber, int portNumber, int abonentNumber)
        {
            DogovorNumber = dogovorNumber;
            PortNumber = portNumber;
            AbonentNumber = abonentNumber;
        }
    }
}
