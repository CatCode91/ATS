using ATSLibrary;
using ATSLibrary.Tariffs;
using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;

namespace ATS
{
    public class Program
    {
        private readonly Random random = new Random();

        static void Main(string[] args)
        {
            //Создаем объект телефонной станции
            Station ats = new Station("VELCOM");
            Console.WriteLine();
            Menu(ats);
        }

        static void Menu(Station ats)
        {
            Abonent[] abonents = CreateDogovors(ats, 10);
            bool isWorking = true;
            while (isWorking)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. Подключить терминал к порту \t 4. Совершить звонок  \t 7. Журнал вызовов");
                Console.WriteLine("2. Отключить терминал от порта \t 5. Узнать баланс \t 8. Смена тариф. плана");
                Console.WriteLine("3. Подключить все терминалы \t 6. Пополнить счет \t 9. Выйти из программы");
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
                            ConnectTerminalsToPorts(abonents);
                            break;
                        case 4:
                            MakeDial(abonents);
                            break;
                        case 5:
                            GetBalance(abonents);
                            break;
                        case 6:
                            PayBill(ats, abonents);
                            break;
                        case 7:
                            GetHistory(ats, abonents);
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

        private static void PayBill(Station ats, Abonent[] abonents)
        {
            Console.WriteLine("Номер порта абонента из списка");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent currentAbonent = abonents[number];

            Console.WriteLine("Введите сумму:");
            double amount = Convert.ToDouble(Console.ReadLine());
            ats.AddMoney(currentAbonent.Dogovor, amount);
        }

        /// <summary>
        /// Изменить тарифный план
        /// </summary>
        /// <param name="abonents"></param>
        private static void ChangeTariff(Abonent[] abonents)
        {
            Console.WriteLine("Номер порта абонента из списка");
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
            Console.WriteLine("Номер порта абонента из списка");
            int number = Convert.ToInt32(Console.ReadLine());
            Abonent currentAbonent = abonents[number];

            List<Call> history = ats.GetHistory(currentAbonent.Port.AbonentNumber);

            Console.WriteLine("    Дата вызова   \t --\t \t Длительность  \t Стоимость");

            foreach (var s in history)
            {
                Console.WriteLine($"{s.StartDate} \t {s.AbonentFrom}=>{s.AbonentTo} \t {s.Duration.ToString(@"hh\:mm\:ss")} \t {s.Amount} BYN");
            }

           Console.WriteLine("Фильтровать по:");
           Console.WriteLine("1. Дате \t 2. Сумме  \t 3. Номеру");

            bool isAlive = true;

            while (isAlive)
            {
                int command = Convert.ToInt32(Console.ReadLine());

                switch (command)
                {
                    case 1:
                        ApplyFilter(Filter.FilterByDate);
                        break;

                    case 2:
                        ApplyFilter(Filter.FilterByAmount);
                        break;

                    case 3:
                        ApplyFilter(Filter.FilterByDate);
                        break;
                    case 4:
                        ApplyFilter(Filter.FilterReset);
                        break;
                    case 5:
                        isAlive = false;
                        return;
                }
            }
        }

        private static void ApplyFilter(Filter filterByDate)
        {
           



        }

        /// <summary>
        /// Узнать баланс
        /// </summary>
        /// <param name="abonents"></param>
        private static void GetBalance(Abonent[] abonents)
        {
            Console.WriteLine("Позвоните по номеру 100 :)");
        }

        /// <summary>
        /// Совершить звонок
        /// </summary>
        /// <param name="abonents"></param>
        private static void MakeDial(Abonent[] abonents)
        {
            Console.WriteLine("Номер порта абонента из списка");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent abonent = abonents[number];

            Console.WriteLine("Введите номер для звонка:");
            int dialNumber = Convert.ToInt32(Console.ReadLine());
            abonent.Terminal.Dial(dialNumber);
            Console.ReadKey();
            abonent.Terminal.FinishDial();
        }

        /// <summary>
        /// Подключение или отключение терминала к порту
        /// </summary>
        /// <param name="abonents"></param>
        /// <param name="isConnect">True - подключить, False - отключить</param>
        private static void SetTerminalToPorts(Abonent[] abonents, bool isConnect)
        {
            Console.WriteLine("Номер порта абонента из списка");
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
        /// <param name="count"></param>
        private static Abonent[] CreateDogovors(Station ats, int count)
        {
            Abonent[] abonents = new Abonent[count];

            for (int i = 0; i < count; i++)
            {
                Dogovor dogovor = ats.CreateDogovor();
                Port port = ats.GetMyPort(dogovor);
                Terminal phone = ats.GetPhone();
                phone.Ringing += Phone_Ringing;
                abonents[i] = new Abonent(dogovor, port, phone);
                Console.WriteLine($"{i} Создан абонент с номером: {port.AbonentNumber} Тариф: {dogovor.Tariff.Name} Модель: {phone.Name}");
            }

            return abonents;
        }

        /// <summary>
        /// Создает указанное количество абонентов
        /// </summary>
        /// <param name="ats"></param>
        /// <param name="count"></param>
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
            Console.WriteLine($"Ответить - 1, Сбросить - 2");

            var command = int.Parse(Console.ReadLine());

            switch (command)
            {
                case 1:
                    Console.WriteLine("Нажмите любую кнопку для завершения вызова");
                    sender.SendAcceptCall(true);
                    break;
                case 2:
                    sender.SendAcceptCall(false);
                    break;
            }
        }

        enum Filter
        {
            FilterByDate,
            FilterByAmount,
            FilterByAbonent,
            FilterReset
        }
    }
}
