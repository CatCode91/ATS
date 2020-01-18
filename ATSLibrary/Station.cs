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
        private Port[] ports = new Port[10];
        //список заключенных договоров
        private List<Dogovor> dogovors = new List<Dogovor>();
        //словарь соответствия номеров договоров к портам
        private Dictionary<Dogovor, Port> dogovorMap = new Dictionary<Dogovor, Port>();

        public Station(string name)
        {
            Console.WriteLine("Станция запускается...");
            CompanyName = name;
            InitializePorts();
            Console.WriteLine("Станция запущена!");
        }

        public string CompanyName { get; }

        /// <summary>
        /// Заключить новый договор
        /// </summary>
        /// <param name="tariff"></param>
        /// <returns></returns>
        public Dogovor CreateDogovor(Tariff tariff)
        {
            int number = dogovors.Count() + 1;

            Dogovor dogovor = new Dogovor(number, tariff);
            Port port = ports.First(x => x.Status == PortStatus.Free);
            int portNumber = Array.IndexOf(ports, port);
            port.SetAbonentNumber(portNumber,29000 + portNumber);
            dogovorMap.Add(dogovor,port);

            return dogovor;
        }

        /// <summary>
        /// Получить порт согласно договора для подключения пользовательских терминалов
        /// </summary>
        /// <param name="dogovor"></param>
        /// <returns></returns>
        public Port GetMyPort(Dogovor dogovor)
        {
            Port port = dogovorMap[dogovor];

            if (dogovor.IsPortSet)
            {
                throw new Exception(message:$"{port.PortNumber}: Порт зарегистрирован и уже используется!");
            }

            dogovor.IsPortSet = true;
            return port;
        }

        private void InitializePorts()
        {
            Console.WriteLine("Инициализация портов...");

            for (int i = 0; i <ports.Length; i++)
            {
                Console.Write("#");
                ports[i] = new Port();
                ports[i].PortConnected += Station_PortConnected;
                ports[i].PortDisconnected += Station_PortDisconnected;
            }
            Console.WriteLine();
            Console.WriteLine("Инициализация завершена");
        }

        private void Station_PortConnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall += Sender_OutcomeCall;
            sender.CallAccepted += Sender_CallAccepted;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Connected);
        }

        private void Station_PortDisconnected(Port sender, PortEventArgs e)
        {
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Disconnected);
        }

        private void Sender_CallAccepted(Port sender, PortEventArgs e)
        {
            Console.WriteLine($"Вызов подтвержден {e.DialNumber} {sender.AbonentNumber}");
        }

        private void Sender_OutcomeCall(Port sender, PortEventArgs e)
        {
            //ищем порт соответствующий вызываемому номеру
            Port callingPort = ports.FirstOrDefault(x => x.AbonentNumber == e.Number);

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
