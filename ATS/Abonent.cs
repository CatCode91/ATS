using ATSLibrary;
using ATSLibrary.Terminals;

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

        public Dogovor Dogovor { get; }
        public Port Port { get; }
        public Terminal Terminal { get; }
    }
}
