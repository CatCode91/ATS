using System;
using System.Collections.Generic;
using System.Globalization;
using ATSLibrary;
using ATSLibrary.Tariffs;
using ATSLibrary.Terminals;

namespace ATS
{
    public class Program
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Перечисления для фильтра результатов истории звонков
        /// </summary>
        private enum Filter
        {
            FilterByDate,
            FilterByAmount,
            FilterByAbonent,
            FilterReset
        }

        static void Main(string[] args)
        {
            //Создаем объект телефонной станции
            Station ats = new Station("VELCOM");
            Console.WriteLine();
            Menu(ats);
        }

        /// <summary>
        /// Основное меню программы
        /// </summary>
        /// <param name="ats"></param>
        static void Menu(Station ats)
        {
            Abonent[] abonents = CreateDogovors(ats, 10);

            bool isWorking = true;

            while (isWorking)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. Подключить терминал \t 4. Узнать баланс  \t 7. Расчет задолженностей");
                Console.WriteLine("2. Отключить терминал \t 5. Пополнить счет  \t 8. Смена тарифного плана");
                Console.WriteLine("3. Совершить звонок \t 6. Журнал вызовов \t 9. Выйти из программы ");
                Console.WriteLine();
                Console.WriteLine("Пункт меню:");

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            SetTerminalToPorts(abonents, true);
                            break;
                        case 2:
                            SetTerminalToPorts(abonents, false);
                            break;
                        case 3:
                            MakeDial(abonents);
                            break;
                        case 4:
                            GetBalance(abonents);
                            break;
                        case 5:
                            AddMoney(ats, abonents);
                            break;
                        case 6:
                            GetHistory(ats, abonents);
                            break;
                        case 7:
                            ats.CountDebts();
                            break;
                        case 8:
                            ChangeTariff(abonents);
                            break;
                        case 9:
                            isWorking = false;
                            continue;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Подбить долги всех абонентов
        /// </summary>
        /// <param name="ats"></param>
        private static void CountDebts(Station ats)
        {
            ats.CountDebts();
        }

        /// <summary>
        /// Пополнить баланс
        /// </summary>
        /// <param name="ats"></param>
        /// <param name="abonents"></param>
        private static void AddMoney(Station ats, Abonent[] abonents)
        {
            Console.WriteLine("Номер договора");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent currentAbonent = abonents[number];

            Console.WriteLine("Введите сумму:");
            decimal amount = Convert.ToDecimal(Console.ReadLine());
            //урезаем до сотых (копеек)
            amount = Math.Truncate(100 * amount) / 100;
            ats.AddMoney(currentAbonent.Dogovor, amount);
        }

        /// <summary>
        /// Изменить тарифный план
        /// </summary>
        /// <param name="abonents"></param>
        private static void ChangeTariff(Abonent[] abonents)
        {
            Console.WriteLine("Номер договора");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent currentAbonent = abonents[number];

            Console.WriteLine($"Ваш текущий тариф:{currentAbonent.Dogovor.Tariff.Name}" + Environment.NewLine);
            Console.WriteLine("Доступные тарифные планы: " + Environment.NewLine);

            //не заморачивался тут с рефлексией для полного списка тарифных планов
            Type baseType = typeof(Tariff);

            Tariff[] tariffs = new Tariff[3];
            tariffs[0] = new EasyTariff();
            tariffs[1] = new MediumTariff();
            tariffs[2] = new FullTariff();

            Console.WriteLine("Выберите номер нового тарифного плана: ");

            for (int i = 0; i < tariffs.Length; i++)
            {
                Console.WriteLine($"{i} {tariffs[i].Name}");
            }

            int current = Convert.ToInt32(Console.ReadLine());
            currentAbonent.Dogovor.ChangeTariff(tariffs[current]);
        }

        /// <summary>
        /// Получить историю звоноков абонента
        /// </summary>
        /// <param name="ats"></param>
        /// <param name="abonents"></param>
        private static void GetHistory(Station ats, Abonent[] abonents)
        {
            Console.WriteLine("Номер договора");
            int number = Convert.ToInt32(Console.ReadLine());
            Abonent currentAbonent = abonents[number];

            List<Call> history = ats.GetHistory(currentAbonent.Port.AbonentNumber);

            if (history == null)
            {
                Console.WriteLine($"{currentAbonent.Port.AbonentNumber} - нет истории вызовов");
                return;
            }

            Console.WriteLine("    Дата вызова   \t --\t \t Длительность  \t Стоимость");

            foreach (var s in history)
            {
                Console.WriteLine($"{s.StartDate} \t {s.AbonentFrom}=>{s.AbonentTo} \t {s.Duration.ToString(@"hh\:mm\:ss")} \t {s.Amount} BYN");
            }

            bool isAlive = true;

            while (isAlive)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Фильтровать по:");
                Console.WriteLine($"1. Дате \t 3. Номеру \t 5.Выход");
                Console.WriteLine($"2. Сумме \t 4. Сброс фильтра");
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            ApplyFilter(history, Filter.FilterByDate);
                            break;
                        case 2:
                            ApplyFilter(history, Filter.FilterByAmount);
                            break;
                        case 3:
                            ApplyFilter(history, Filter.FilterByAbonent);
                            break;
                        case 4:
                            ApplyFilter(history, Filter.FilterReset);
                            break;
                        case 5:
                            isAlive = false;
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Применить фильтр к коллекции
        /// </summary>
        /// <param name="history"></param>
        /// <param name="filter"></param>
        private static void ApplyFilter(List<Call> history, Filter filter)
        {
            List<Call> result = new List<Call>();

            if (!Enum.IsDefined(typeof(Filter), filter))
            {
                return;
            }

            switch (filter)
            {
                case Filter.FilterByDate:
                    Console.WriteLine("Введите дату(дд.мм.гг):");
                    DateTime date = DateTime.ParseExact(Console.ReadLine().ToString(), "dd.MM.yy", CultureInfo.InvariantCulture);
                    result = history.FindAll(x => x.StartDate.Date == date.Date);
                    break;

                case Filter.FilterByAmount:
                    Console.WriteLine("Введите сумму:");
                    decimal amount = Convert.ToDecimal(Console.ReadLine());
                    result = history.FindAll(x => x.Amount == amount);
                    break;

                case Filter.FilterByAbonent:
                    Console.WriteLine("Введите номер абонента:");
                    decimal number = Convert.ToDecimal(Console.ReadLine());
                    result = history.FindAll(x => x.AbonentFrom == number || x.AbonentTo == number);
                    break;

                case Filter.FilterReset:
                    result = history;
                    break;
            }

            if (result.Count == 0)
            {
                Console.WriteLine("Нет результатов");
            }

            foreach (var s in result)
            {
                Console.WriteLine($"{s.StartDate} \t {s.AbonentFrom}=>{s.AbonentTo} \t {s.Duration.ToString(@"hh\:mm\:ss")} \t {s.Amount} BYN");
            }

            result = null;
        }

        /// <summary>
        /// Узнать баланс
        /// </summary>
        /// <param name="abonents"></param>
        private static void GetBalance(Abonent[] abonents)
        {
            Console.WriteLine("Позвоните по номеру 100 :)");
            Console.ReadKey();
        }

        /// <summary>
        /// Совершить звонок
        /// </summary>
        /// <param name="abonents"></param>
        private static void MakeDial(Abonent[] abonents)
        {
            Console.WriteLine("Номер договора");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent abonent = abonents[number];

            Console.WriteLine("Введите номер для звонка:");
            int dialNumber = Convert.ToInt32(Console.ReadLine());
            abonent.Terminal.Dial(dialNumber);
            Console.ReadKey();
            abonent.Terminal.FinishDial();
            Console.ReadKey();
        }

        /// <summary>
        /// Подключение или отключение терминала к порту
        /// </summary>
        /// <param name="abonents"></param>
        /// <param name="isConnect">True - подключить, False - отключить</param>
        private static void SetTerminalToPorts(Abonent[] abonents, bool isConnect)
        {
            Console.WriteLine("Номер договора");

            int number = Convert.ToInt32(Console.ReadLine());

            Abonent currentAbonent = abonents[number];

            if (isConnect)
            {
                currentAbonent.Terminal.ConnectPort(currentAbonent.Port);
            }

            else
            {
                currentAbonent.Terminal.DisconnectPort();
            }
        }

        /// <summary>
        /// Создает указанное количество абонентов
        /// </summary>
        /// <param name="ats"></param>
        /// <param name="count">Количество абонентов</param>
        private static Abonent[] CreateDogovors(Station ats, int count)
        {
            Abonent[] abonents = new Abonent[count];

            for (int i = 0; i < count; i++)
            {
                Dogovor dogovor = ats.CreateDogovor();
                Port port = ats.GetPort(dogovor);
                Terminal phone = ats.GetPhone();
                phone.Ringing += Phone_Ringing;
                abonents[i] = new Abonent(dogovor, port, phone);
                Console.WriteLine($"{i} - Договор №{dogovor.DogovorNumber} Номер: {port.AbonentNumber} Тариф: {dogovor.Tariff.Name} Терминал: {phone.Name}");
            }

            Console.WriteLine();
            //сразу подключаем всех к портам
            ConnectTerminalsToPorts(abonents);

            return abonents;
        }

        /// <summary>
        /// Подключает все терминалы из массива к портам
        /// </summary>
        private static void ConnectTerminalsToPorts(Abonent[] abonents)
        {
            for (int i = 0; i < abonents.Length; i++)
            {
                abonents[i].Terminal.ConnectPort(abonents[i].Port);
            }
        }

        /// <summary>
        /// Оповещение о входящем вызове
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Phone_Ringing(ITerminal sender, TerminalEventArgs e)
        {
            Console.WriteLine($"{sender.Name}: {e.Message}");
            Console.WriteLine($"Ответить - 1, Сбросить - любая клавиша");

            int command = int.Parse(Console.ReadLine());
            bool answer = false;

            if (command == 1)
            {
                answer = true;
            }

            sender.SendAcceptCall(answer);
        }
    }
}
