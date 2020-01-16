using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Port
    {
        private int _portNumber;
        //абоненский номер на порту
        private int _abonentNumber;
        //терминал, подключенный к порту
        private ITerminal _terminal;
        //текущий звонок на порту
        private Call currentCall;
        //история звонков
        private List<Call> callsHistory;

        internal event PortStateHandler PortConnected;
        internal event PortStateHandler PortDisconnected;
        internal event PortStateHandler CallNotify;
        internal event PortStateHandler RingNotify;

        internal Port()
        {
            Status = PortStatus.Free;
        }


        public int AbonentNumber => _abonentNumber;
        public PortStatus Status { get; private set; }

        internal int PortNumber => _portNumber;

        internal void ConnectTerminal(ITerminal terminal)
        {
            if (_terminal == null)
            {
                _terminal = terminal;
                PortConnected?.Invoke(this, new PortEventArgs($"Port №{PortNumber}:  Устройство подключено к порту!"));
            }

            else
            {
                Console.WriteLine($"Port №{PortNumber}:  Порт уже используется другим устройством!");
            }

           
        }      
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

        internal void IncomeCalling(int incomeNumber)
        {
            RingNotify?.Invoke(this, new PortEventArgs($"Входящий вызов от абонента {incomeNumber}")); ;
        }
        internal void OutcomeCalling(int number)
        {
            CallNotify?.Invoke(this, new PortEventArgs(number,Status));   
        }

        internal void Talking()
        {
            Status = PortStatus.Busy;
        }

        internal void BusySent()
        {

        }

        internal void FinishTalking(Call call)
        {
            callsHistory.Add(call);
            Status = PortStatus.Connected;
        }

        internal void SetAbonentNumber(int portNumber,int abonentNumber)
        {
            _portNumber = portNumber;
            _abonentNumber = abonentNumber;
            Status = PortStatus.Disconnected;  
        }

        internal void PortStatusChange(PortStatus status)
        {
            Status = status;
        }
    }
}
