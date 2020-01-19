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
        private Random random = new Random();
        private string[] phoneModels = { "Alcatel", "BlackBerry", "iPhone", "Motorola", "Nokia", "Samsung", "Xiaomi" };

        private bool _acceptedCall;

        private Billing billing = new Billing();
        //количество портов на станции
        private Port[] ports = new Port[30];
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

            //заключаем договор согласно выбранного тарифа, но пока подкинем тариф рандомно
            Dogovor dogovor = new Dogovor(number, RandomTariff());

            //ищем свободный порт и занимаем его
            Port port = ports.First(x => x.Status == PortStatus.Free);
            port.PortStatusChange(PortStatus.Disconnected);

            int portNumber = Array.IndexOf(ports, port);

            port.SetAbonentNumber(number, portNumber, 29000 + portNumber);

            dogovors.Add(dogovor);

            dogovorMap.Add(dogovor, port);
           

            return dogovor;
        }

        public Phone GetPhone()
        {
            int index = random.Next(0, phoneModels.Length);

            return new Phone(phoneModels[index]);
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
                //для красоты :) типа грузятся порты)
                Thread.Sleep(50);
            }
            Console.WriteLine();
         
            Console.WriteLine("Инициализация завершена");
            Console.WriteLine();
        }

        private void Station_PortConnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall += Sender_OutcomeCall;
            sender.CallAccepted += Sender_CallAccepted;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Connected);
        }

        private bool Sender_CallAccepted(bool accept)
        {
            _acceptedCall = accept;
            return accept;
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
            if (_acceptedCall)
            {
                Console.WriteLine($"Вызов принят абонентом {sender.AbonentNumber} ");
                Connection(sender, calledPort);
            }

            else
            {
                Console.WriteLine($"Вызов отклонен абонентом {sender.AbonentNumber}");
            }

        }

        private async void Connection(Port callingPort, Port answerPort)
        {
            Console.WriteLine("Начат разговор");
            DateTime timeStart = DateTime.Now;
            await Task.Run(() => Talking(callingPort, answerPort));
            FinishDialog(callingPort, answerPort, timeStart);
        }

        private void FinishDialog(Port callingPort, Port answerPort, DateTime timeStart)
        {
            DateTime timeFinish = DateTime.Now; 
            Tariff tariff = dogovors.FirstOrDefault(x => x.DogovorNumber == callingPort.DogovorNumber).Tariff;
            billing.AddCall(new Call(timeStart,timeFinish,tariff, callingPort.AbonentNumber, answerPort.AbonentNumber));
            Console.WriteLine("Запись о звонке добавлена в журнал");
        }

        private void Talking(Port call, Port answer)
        {
            //переменная для импровизированного счетчика чисто для вывода на консоль
            int i = 0;

            while (true)
            {             
                if (call.CancelTokenSource.Token.IsCancellationRequested)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Вызов завершен абонентом {call.AbonentNumber}");
                    return;
                }

                if (answer.CancelTokenSource.Token.IsCancellationRequested)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Вызов завершен абонентом {answer.AbonentNumber}");
                    return;
                }

                TimeSpan time = new TimeSpan(0, 0, i);
                Console.Write(time);
                Thread.Sleep(1000);
                i++;
                Console.CursorLeft = 0;
            }
        }

        private Tariff RandomTariff()
        {
            Type baseType = typeof(Tariff);
            var allDerivedTypes = baseType.Assembly.ExportedTypes.Where(t => baseType.IsAssignableFrom(t)).Where(t => t.IsAbstract == false).ToArray();
            Tariff tariff = Activator.CreateInstance(Type.GetType(allDerivedTypes[random.Next(0, allDerivedTypes.Length)].FullName)) as Tariff;
            return tariff;
        }

    }
}
