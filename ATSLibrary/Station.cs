using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Station
    {
        //количество портов на станции
        private Port[] ports = new Port[100];
        //список заключенных договоров
        private List<Dogovor> dogovors = new List<Dogovor>();
        //словарь номеров договоров к портам
        private Dictionary<Dogovor, Port> dogovorMap = new Dictionary<Dogovor, Port>();

        public Station(string name)
        {
            Console.WriteLine("Станция запускается...");
            CompanyName = name;
            InitializePorts();
            Console.WriteLine("Станция запущена!");
        }

        public string CompanyName { get; }

        public Dogovor CreateDogovor(Tariff tariff)
        {
            int number = dogovors.Count() + 1;

            Dogovor dogovor = new Dogovor(number, tariff);
            Port port = ports.First(x => x.Status == PortStatus.Free);
            port.SetAbonentNumber(Array.IndexOf(ports,port),29000);
            dogovorMap.Add(dogovor,port);

            return dogovor;
        }

        public Port GetPort(Dogovor dogovor)
        {
            Port port = dogovorMap[dogovor];

            if (dogovor.IsPortSet)
            {
                throw new Exception(message:$"{port.PortNumber}: Порт уже используется!");
            }

            dogovor.IsPortSet = true;
            return port;
        }

        private void InitializePorts()
        {
            Console.WriteLine("Инициализация портов...");

            for (int i = 0; i <ports.Length; i++)
            {
                ports[i] = new Port();
                ports[i].PortConnected += Station_PortConnected;
                ports[i].PortDisconnected += Station_PortDisconnected;
                ports[i].CallNotify += Station_CallNotify;
            }
        }

        private void Station_PortConnected(Port sender, PortEventArgs e)
        {
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Connected);
        }

        private void Station_PortDisconnected(Port sender, PortEventArgs e)
        {
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Disconnected);
        }

        private void Station_CallNotify(Port sender, PortEventArgs e)
        {
            var callingPort = ports.First(x => x.AbonentNumber == e.DialNumber);

            if (callingPort == null)
            {
                Console.WriteLine("Вызываемого Вами абонента не существует");
                return;
            }

            if (callingPort.Status == PortStatus.Busy)
            {
                Console.WriteLine("Вызываемый Вами абонент занят");
                return;
            }

            if (callingPort.Status == PortStatus.Disconnected)
            {
                Console.WriteLine("Вызываемый Вами абонент недоступен");
                return;
            }

            Console.WriteLine("Вызов абонента...");
            callingPort.IncomeCalling(sender.AbonentNumber);
        }
   
    }
}
