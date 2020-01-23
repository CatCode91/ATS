using ATSLibrary.Tariffs;
using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Station
    {
        private Random _random = new Random();
        //объект магазина или склада (там хранятся терминалы)
        private Store _store = new Store();
        //поле турпл, подтвердил ли вызов вызываемый абонент
        private (bool,Port) _acceptedCall;
        //объект биллинговой системы
        private Billing _billing = new Billing();
        //количество возможных портов на станции
        private Port[] _ports = new Port[30];
        //список заключенных договоров
        private List<Dogovor> _dogovors = new List<Dogovor>();
        //словарь соответствия номеров договоров к портам
        private Dictionary<Dogovor, Port> _dogovorMap = new Dictionary<Dogovor, Port>();
        //словарь соответствия номеров телефонов для вызова служебных функций
        private Dictionary<int, Action<Dogovor>> _systemMethods = new Dictionary<int, Action<Dogovor>>();

        public Station(string name)
        {
            Console.WriteLine("Станция запускается...");
            CompanyName = name;

            InitializePorts();
            InitializeSystemNumbers();

            Console.WriteLine("Станция запущена!");
            Console.WriteLine();
        }

        public string CompanyName
        {
            get;
        }

        /// <summary>
        /// Заключить новый договор
        /// </summary>
        /// <param name="tariff"></param>
        /// <returns></returns>
        public Dogovor CreateDogovor()
        {
            int dogovorNumber = _dogovors.Count();

            //(пока подкинем тариф рандомно)
            Dogovor dogovor = new Dogovor(dogovorNumber, RandomTariff());

            //ищем первый свободный порт и занимаем его
            Port port = _ports.First(x => x.Status == PortStatus.Free);

            if (port == null)
            {
                throw new Exception(message: "Закончились свободные порты");
            }

            port.PortStatusChange(PortStatus.Disconnected);
            //получаем индекс порта в массиве и делаем индекс порядковым номером порта
            int portNumber = Array.IndexOf(_ports, port);
            //привязываем номер договора, абонентский и порядковый номера к порту
            port.SetAbonentNumber(dogovorNumber, portNumber, 29000 + portNumber);
            //добавляем договор в список заключенных договоров
            _dogovors.Add(dogovor);
            //добавляем в словарь соответствие договора порту
            _dogovorMap.Add(dogovor, port);

            return dogovor;
        }

        /// <summary>
        /// Выдает абоненту терминал
        /// </summary>
        /// <returns></returns>
        public Phone GetPhone()
        {
            return _store.GetPhone();
        }

        /// <summary>
        /// Посчитать задолженность по абонентам
        /// </summary>
        public void CountDebts()
        {
            _billing.CountDebtsForAbonents(_dogovors);
        }

        /// <summary>
        /// Получить порт для подключения пользовательских терминалов согласно договора 
        /// </summary>
        /// <param name="dogovor"></param>
        /// <returns></returns>
        public Port GetPort(Dogovor dogovor)
        {
            Port port = _dogovorMap[dogovor];
            return port;
        }

        /// <summary>
        /// Показать историю вызовов
        /// </summary>
        /// <param name="abonentNumber"></param>
        public List<Call> GetHistory(int abonentNumber)
        {
            return  _billing.GetHistory(abonentNumber);
        }

        /// <summary>
        /// Пополнить баланс
        /// </summary>
        /// <param name="dogovor"></param>
        /// <param name="amount"></param>
        public void AddMoney(Dogovor dogovor, decimal amount)
        {
            Port port = _dogovorMap[dogovor];
            Console.Write($"{port.AbonentNumber}: ");
            dogovor.PayBills(amount);
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
        /// Инициализация системных номеров
        /// </summary>
        private void InitializeSystemNumbers()
        {
            _systemMethods.Add(100, ShowBalance);
        }

        /// <summary>
        /// Показать баланс абонента
        /// </summary>
        /// <param name="dogovor"></param>
        private void ShowBalance(Dogovor dogovor)
        {
            Port port = _dogovorMap[dogovor];
            decimal balance = dogovor.Balance;
            Console.WriteLine($"Баланс абонента {port.AbonentNumber}: {balance} BYN");
            Console.WriteLine($"Задолженность составляет: {dogovor.Debt} BYN");
            Console.WriteLine($"Метод расчета: кредитный");
            Console.WriteLine($"Тарифный план: {dogovor.Tariff.Name} ( {dogovor.Tariff.Rate} BYN за сек. )");
            Console.WriteLine($"Расчет стоимости услуг за текущий месяц производится {_billing.LastPayDay} числа следующего месяца");
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
        /// Метод для события, возникающего при отключения терминала от порта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Station_PortDisconnected(Port sender, PortEventArgs e)
        {
            sender.OutcomeCall -= Sender_OutcomeCall;
            sender.CallAccepted -= Sender_CallAccepted;
            Console.WriteLine(e.Message);
            sender.PortStatusChange(PortStatus.Disconnected);
        }

        /// <summary>
        /// Событие ответа вызываемого абонента на вызов
        /// </summary>
        /// <param name="accept">Принял абонент вызов или отклонил</param>
        /// <returns></returns>
        private void Sender_CallAccepted(Port sender, bool accept)
        {
            if (accept)
            {
                sender.PortStatusChange(PortStatus.Busy);
            }

            _acceptedCall = (accept,sender);
        }

        /// <summary>
        /// Метод на событие исходящего вызова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sender_OutcomeCall(Port sender, PortEventArgs e)
        {
            Dogovor dogovor = _dogovors.FirstOrDefault(x => x.DogovorNumber == sender.DogovorNumber);

            //если был набран служебный номер, вызываем метод из словаря служебных номеров
            if (_systemMethods.ContainsKey(e.AbonentNumber))
            {
                _systemMethods[e.AbonentNumber](dogovor);
                return;
            }

            bool isPaid = _billing.IsBillsPaid(dogovor.Debt);
            //если неоплачены счета, выходим
            if (!isPaid)
            {
                Console.WriteLine($"Оплатите долг! {dogovor.Debt} BYN");
                return;
            }

            //ищем порт соответствующий вызываемому номеру
            Port calledPort = _ports.FirstOrDefault(x => x.AbonentNumber == e.AbonentNumber);

            //если номера не существует на станции, выходим
            if (calledPort == null)
            {
                Console.WriteLine("Вызываемого Вами абонента не существует");
                return;
            }

            //если номер занят, выходим
            if (calledPort.Status == PortStatus.Busy)
            {
                Console.WriteLine("Вызываемый Вами абонент занят");
                return;
            }

            //если номер отключен, выходим
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

            //завершаем разговор после выполнения таски
            FinishDialog(callingPort, answerPort, timeStart);
        }

        /// <summary>
        /// Метод имитирующий разговор, ожидающий завершения разговора любым из абонентов
        /// </summary>
        /// <param name="call"></param>
        /// <param name="answer"></param>
        private void Talking(Port call, Port answer)
        {
            //переменная для импровизированного счетчика секунд, чисто для вывода на консоль
            int i = 1;
            //токен 1го абонента для отмены вызова (завершения таски)
            call.CancelTokenSource = new CancellationTokenSource();
            //токен 2го абонента для отмены вызова (завершения таски)
            answer.CancelTokenSource = new CancellationTokenSource();

            while (true)
            {
                if (call.CancelTokenSource.Token.IsCancellationRequested || call.Status == PortStatus.Disconnected)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Вызов завершен абонентом {call.AbonentNumber}");
                    return;
                }

                if (answer.CancelTokenSource.Token.IsCancellationRequested || answer.Status == PortStatus.Disconnected)
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
            //регистрируем время завершения вызова
            DateTime timeFinish = DateTime.Now;

            //освобождаем токены для прерывания разговора
            callingPort.CancelTokenSource = null;
            answerPort.CancelTokenSource = null;

            //если вызов завершен корректно, то меняем статус
            if (callingPort.Status == PortStatus.Busy)
            {
                callingPort.PortStatusChange(PortStatus.Connected);
            }

            if (answerPort.Status == PortStatus.Busy)
            {
                answerPort.PortStatusChange(PortStatus.Connected);
            }

            //заносим вызов в журнал
            Dogovor dogovor = _dogovors.FirstOrDefault(x => x.DogovorNumber == callingPort.DogovorNumber);
            Tariff tariff = dogovor.Tariff;
            decimal amount =  _billing.GetCallPrice(tariff,timeFinish - timeStart);
            amount = Math.Round(amount, 2);
            Call call = new Call(dogovor.DogovorNumber, tariff, timeStart, timeFinish, callingPort.AbonentNumber, answerPort.AbonentNumber, amount);
            _billing.AddCallToJournal(call);
            Console.WriteLine($"Звонок завершен. Стоимость звонка {amount} BYN");
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
    }
}

