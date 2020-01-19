using ATSLibrary;
using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Abonent
    {
        public Abonent(Dogovor dogovor, Port port, Terminal terminal)
        {
            Dogovor = dogovor;
            Port = port;
            Terminal = terminal;
        }

        Dogovor Dogovor { get; }
        Port Port { get; }
        Terminal Terminal { get; }
    }
}
