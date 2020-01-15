using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port
    {
        //терминал, подключенный к порту
        private ITerminal _terminal;
        //текущий звонок на порту
        private Call currentCall;
        //история звонков
        private List<Call> callsHistory;

        public event PortStateHandler StateChanging;

        public Port()
        {
            Status = PortStatus.Free;
        }

        public Port(Dogovor dogovor)
        {
            Number = dogovor.DogovorNumber;
            Status = PortStatus.Disconnected;
        }

        public int Number { get; }
        public PortStatus Status { get; private set; }

        public void ConnectToPort(ITerminal terminal)
        {
            if(_terminal == null)
            {
               _terminal = terminal;
            }

            StateChanging?.Invoke(this, new PortEventArgs(Status));
            Status = PortStatus.Connected;
        }

        public void DisconnectPort(ITerminal terminal)
        {
            terminal = null;
            Status = PortStatus.Disconnected;
        }

        public void Dialing(int number)
        {
            Status = PortStatus.Busy;
        }

        public void Calling(int number)
        {
            Status = PortStatus.Busy;
        }

        public void Talking()
        {
            Status = PortStatus.Busy;
        }

        public void FinishTalking(Call call)
        {
            callsHistory.Add(call);
            Status = PortStatus.Disconnected;
        }

    }
}
