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
        private Random _random = new Random();
        //список выдаваемых станцией моделей телефонов
        private string[] _phoneModels = { "Alcatel", "BlackBerry", "iPhone", "Motorola", "Nokia", "Samsung", "Xiaomi" };
        //поле, подтвердил ли вызов вызываемый абонент
        private (bool,Port) _acceptedCall;
        //объект биллинговой системы
        private Billing _billing = new Billing();
        //количество возможных портов на станции
        private Port[] _ports = new Port[30];
        //список заключенных договоров
        private List<Dogovor> _dogovors = new List<Dogovor>();
        //словарь соответствия номеров договоров к портам
        private Dictionary<Dogovor, Port> _dogovorMap = new Dictionary<Dogovor, Port>();

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
            int number = _dogovors.Count() + 1;

            //заключаем договор согласно выбранного тарифа (пока подкинем тариф рандомно)
            Dogovor dogovor = new Dogovor(number, RandomTariff());

            //ищем свободный порт и занимаем его
            Port port = _ports.First(x => x.Status == PortStatus.Free);
            port.PortStatusChange(PortStatus.Disconnected);
            int portNumber = Array.IndexOf(_ports, port);
            port.SetAbonentNumber(number, portNumber, 29000 + portNumber);
            _dogovors.Add(dogovor);
            _dogovorMap.Add(dogovor, port);
          
            return dogovor;
        }

        /// <summary>
        /// Выдает абоненту терминал
        /// </summary>
        /// <returns></returns>
        public Phone GetPhone()
        {
            int index = _random.Next(0, _phoneModels.Length);
            return new Phone(_phoneModels[index]);
        }

        /// <summary>
        /// Получить порт для подключения пользовательских терминалов согласно договора 
        /// </summary>
        /// <param name="dogovor"></param>
        /// <returns></returns>
        public Port GetMyPort(Dogovor dogovor)
        {
            Port port = _dogovorMap[dogovor];

            if (dogovor.IsPortSet)
            {
                throw new Exception(message: $"{port.PortNumber}: Порт зарегистрирован и уже используется!");
            }

            dogovor.IsPortSet = true;
            return port;
        }

        public void ShowHistory(int abonentNumber)
        {
            var history = _billing.GetHistory(abonentNumber);

            foreach (var s in history)
            {
                Console.WriteLine($"{s.StartDate}|{s.AbonentFrom}=>{s.AbonentTo}|{s.Duration.ToString("HH:mm:ss")}|{s.Amount}BYN");
            }
        }

        /// <summary>
        /// Инициализация портов (подписка портов на события)
        /// </summary>
        private void InitializePorts()
        {
            Console.WriteLine("Инициализация портов...");

            for (int i = 0; i < _ports.Length; i++)
            {
                Console.Write("#");
                _ports[i] = new Port();
                _ports[i].PortConnected += Station_PortConnected;
                _ports[i].PortDisconnected += Station_PortDisconnected;
                //для красоты :) типа задержка при загрузке))
                Thread.Sleep(50);
            }
            Console.WriteLine();
         
            Console.WriteLine("Инициализация завершена");
            Console.WriteLine();
        }

        /// <summary>
        /// Метод для события, возникающего при подключении терминала к порту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Station_PortConnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall += Sender_OutcomeCall;
            sender.CallAccepted += Sender_CallAccepted;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Connected);
        }

        /// <summary>
        /// Событие ответа вызываемого абонента на вызов
        /// </summary>
        /// <param name="accept">Принял абонент вызов или отклонил</param>
        /// <returns></returns>
        private void Sender_CallAccepted(Port sender, bool accept)
        {
            _acceptedCall = (accept,sender);
        }

        /// <summary>
        /// Метод для события, возникающего при отключения терминала от порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Station_PortDisconnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall -= Sender_OutcomeCall;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Disconnected);
        }

        /// <summary>
        /// Метод на событие исходящего вызова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sender_OutcomeCall(Port sender, PortEventArgs e)
        {
            //так неправильно, нужно использовать инкапсуляцию, но чисто в целях экономии времени :))
            if (e.AbonentNumber == 100)
            {
                double balance = _dogovors.First(x => x.DogovorNumber == sender.DogovorNumber).Balance;
                Console.WriteLine($"Баланс абонента {sender.AbonentNumber}: {balance} BYN");
                return;
            }

            //ищем порт соответствующий вызываемому номеру
            Port calledPort = _ports.FirstOrDefault(x => x.AbonentNumber == e.AbonentNumber);

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
            if (_acceptedCall.Item1 & _acceptedCall.Item2 == calledPort )
            {
                Console.WriteLine($"Вызов принят абонентом {calledPort.AbonentNumber} ");
                Connection(sender, calledPort);
            }

            else
            {
                Console.WriteLine($"Вызов отклонен абонентом {calledPort.AbonentNumber}");
            }

        }

        /// <summary>
        /// Соединение двух портов
        /// </summary>
        /// <param name="callingPort">Вызывающий порт</param>
        /// <param name="answerPort">Вызываемый порт</param>
        private async void Connection(Port callingPort, Port answerPort)
        {
            Console.WriteLine("Начат разговор");
            DateTime timeStart = DateTime.Now;
            await Task.Run(() => Talking(callingPort, answerPort));
            FinishDialog(callingPort, answerPort, timeStart);
        }

        /// <summary>
        /// Метод имитирующий разговор, ожидающий завершения разговора любым из абонентов
        /// </summary>
        /// <param name="call"></param>
        /// <param name="answer"></param>
        private void Talking(Port call, Port answer)
        {
            //переменная для импровизированного счетчика чисто для вывода на консоль
            int i = 1;
            call.CancelTokenSource = new CancellationTokenSource();
            answer.CancelTokenSource = new CancellationTokenSource();

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

        /// <summary>
        /// Завершение вызова
        /// </summary>
        /// <param name="callingPort"></param>
        /// <param name="answerPort"></param>
        /// <param name="timeStart"></param>
        private void FinishDialog(Port callingPort, Port answerPort, DateTime timeStart)
        {
            callingPort.CancelTokenSource.Dispose();
            answerPort.CancelTokenSource.Dispose();

            DateTime timeFinish = DateTime.Now; 
            Dogovor dogovor = _dogovors.FirstOrDefault(x => x.DogovorNumber == callingPort.DogovorNumber);
            Tariff tariff = dogovor.Tariff;
            double amount = CalculateCallAmount(tariff,timeFinish - timeStart);
            _billing.PayCall(dogovor, amount);
            _billing.AddCall(new Call(tariff, timeStart,timeFinish, callingPort.AbonentNumber, answerPort.AbonentNumber,amount));
            Console.WriteLine($"Звонок завершен. Баланс {callingPort.AbonentNumber}: {dogovor.Balance} BYN");
        }

        /// <summary>
       /// Возвращает рандомный тарифный план
       /// </summary>
       /// <returns></returns>
        private Tariff RandomTariff()
        {
            Type baseType = typeof(Tariff);
            var allDerivedTypes = baseType.Assembly.ExportedTypes.Where(t => baseType.IsAssignableFrom(t)).Where(t => t.IsAbstract == false).ToArray();
            Tariff tariff = Activator.CreateInstance(Type.GetType(allDerivedTypes[_random.Next(0, allDerivedTypes.Length)].FullName)) as Tariff;
            return tariff;
        }

        /// <summary>
        /// Подсчет суммы, потраченной на звонок
        /// </summary>
        /// <param name="tariff">Текущий тариф абонента</param>
        /// <param name="duration">Длительность звонка</param>
        /// <returns></returns>
        private double CalculateCallAmount(Tariff tariff,TimeSpan duration)
        {
            return duration.Seconds * tariff.Rate;
        } 
    }
}
