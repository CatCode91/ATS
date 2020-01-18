using ATSLibrary.Tariffs;
using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Station
    {
        private List<Call> journal = new List<Call>();

        //количество портов на станции
        private Port[] ports = new Port[10];
        //список заключенных договоров
        private List<Dogovor> dogovors = new List<Dogovor>();

        //словарь соответствия номеров договоров к портам
        private Dictionary<Dogovor, Port> dogovorMap = new Dictionary<Dogovor, Port>();

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken token = cancelTokenSource.Token;


        public Station(string name)
        {
            Console.WriteLine("Станция запускается...");
            CompanyName = name;
            InitializePorts();
            Console.WriteLine("Станция запущена!");
            Console.WriteLine();
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
            port.SetAbonentNumber(number,portNumber, 29000 + portNumber);
            dogovorMap.Add(dogovor, port);
            dogovors.Add(dogovor);

            return dogovor;
        }

        public Phone GetPhone(PhoneModels model)
        {
            return new Phone(model);
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
                throw new Exception(message: $"{port.PortNumber}: Порт зарегистрирован и уже используется!");
            }

            dogovor.IsPortSet = true;
            return port;
        }

        private void InitializePorts()
        {
            Console.WriteLine("Инициализация портов...");

            for (int i = 0; i < ports.Length; i++)
            {
                Console.Write("#");
                ports[i] = new Port();
                ports[i].PortConnected += Station_PortConnected;
                ports[i].PortDisconnected += Station_PortDisconnected;
            }
            Console.WriteLine();
            Console.WriteLine("Инициализация завершена");
            Console.WriteLine();
        }

        private void Station_PortConnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall += Sender_OutcomeCall;
            sender.CallFinish += Sender_CallFinish;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Connected);
        }

        private void Sender_CallFinish(Port sender, PortEventArgs e)
        {
            Console.WriteLine($"{sender.AbonentNumber} инициировал завершение разговора");
            cancelTokenSource.Cancel();
        }

        private void Station_PortDisconnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall -= Sender_OutcomeCall;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Disconnected);
        }

        private void Sender_OutcomeCall(Port sender, PortEventArgs e)
        {
            //ищем порт соответствующий вызываемому номеру
            Port calledPort = ports.FirstOrDefault(x => x.AbonentNumber == e.AbonentNumber);

            if (calledPort == null)
            {
                Console.WriteLine("Вызываемого Вами абонента не существует");
                return;
            }

            if (calledPort.Status == PortStatus.Busy)
            {
                Console.WriteLine("Вызываемый Вами абонент занят");
                return;
            }

            if (calledPort.Status == PortStatus.Disconnected)
            {
                Console.WriteLine("Вызываемый Вами абонент недоступен");
                return;
            }

            Console.WriteLine("Вызов абонента...");
            //Вызываем метод вызова на найденом порту
            calledPort.IncomeCalling(sender);

            //проверяем, ответил ли абонент
            if (calledPort.IsCallAccepted)
            {
                Console.WriteLine($"Вызов принят абонентом {calledPort.AbonentNumber} ");
                DateTime timeStart = DateTime.Now;
                Talk(sender, calledPort);
                DateTime timeFinish = DateTime.Now;
                TimeSpan duration = timeFinish - timeStart;
                Tariff currentTariff = dogovors.Find(x => x.DogovorNumber == sender.DogovorNumber).Tariff;
                journal.Add(new Call(timeStart,duration,currentTariff,sender.AbonentNumber,calledPort.AbonentNumber));
            }

            else
            {
                Console.WriteLine($"Вызов отклонен абонентом {calledPort.AbonentNumber}");
            }
        }

        private void Talk(Port call, Port answer)
        {
            //тут происходит соединение двух портов
            Console.WriteLine("Начат разговор");
            Console.WriteLine("Нажмите любую клавишу для отмены...");

            if (token.IsCancellationRequested)
            {
                return;
            }

            Task task1 = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.Write("#");
                }
            });

            Console.WriteLine();
            task1.Start();
        }

    }
}
