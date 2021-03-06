﻿using System;
using System.Threading;

namespace ATSLibrary
{
    public class Port
    {
        /// <summary>
        /// Происходит, когда на порт поступает входящий вызов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="abonentNumber">номер вызывающего абонента</param>
        public delegate void RingAcceptHandle(Port sender, int abonentNumber);
        public event RingAcceptHandle RingNotify;


        internal event PortStateHandler PortConnected;
        internal event PortStateHandler PortDisconnected;
        internal event PortStateHandler OutcomeCall;

        /// <summary>
        /// Подтверждение входящего вызова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="accept"></param>
        internal delegate void СallAcceptHandle(Port sender,bool accept);
        internal event СallAcceptHandle CallAccepted;

        internal Port()
        {
            Status = PortStatus.Free;
        }

        public int AbonentNumber { get; private set; }
        public PortStatus Status { get; private set; }

        internal int PortNumber { get; private set; }
        internal int DogovorNumber { get; private set; }
        
        //токен для отмены активного соединения (разговора)
        internal CancellationTokenSource CancelTokenSource { get; set; }

        /// <summary>
        /// Подключить терминал к порту
        /// </summary>
        /// <param name="terminal"></param>
        internal void ConnectTerminal(ITerminal terminal)
        {
                PortConnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Терминал {terminal.Name} подключен к порту с абон. номером {AbonentNumber}"));
        }      
      
        /// <summary>
        /// Отключить терминал от порта
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="port"></param>
        internal void DisconnectTerminal(ITerminal terminal)
        {
            PortDisconnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Терминал {terminal.Name} отключен от порта с абон. номером {AbonentNumber}"));
        }

        /// <summary>
        /// На порт поступает входящий вызов
        /// </summary>
        /// <param name="incomeNumber"></param>
        internal void IncomeCalling(Port port)
        {
            //оповестить о входящем вызове подписанный терминал
            RingNotify?.Invoke(this, port.AbonentNumber);
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
            CallAccepted?.Invoke(this,accepted);
        }

        /// <summary>
        /// Закончить разговор
        /// </summary>
        internal void FinishTalking()
        {
            CancelTokenSource.Cancel();
        }

        /// <summary>
        /// Изменяет статус порта (лучше использовать только на станции!)
        /// </summary>
        /// <param name="status"></param>
        internal void PortStatusChange(PortStatus status)
        {
            if (Enum.IsDefined(typeof(PortStatus), status))
            {
                Status = status;
            }  
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
