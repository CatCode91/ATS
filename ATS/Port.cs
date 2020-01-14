using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Port
    {
        private int _number;
        private PortStatus _status;
        private ITerminal _terminal;
        private Call currentCall;

        private List<Call> callsHistory;

        public Port(Dogovor dogovor)
        {
            _number = dogovor.DogovorNumber;
            callsHistory = new List<Call>();
        }

        public int Number => _number;
        public PortStatus Status { get; private set; }

        public void Connect(ITerminal terminal)
        {
            if(terminal == null)
            {
                terminal = _terminal;
            }

            Status = PortStatus.Connected;
        }

        public void Disconnect(ITerminal terminal)
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
