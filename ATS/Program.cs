using ATSLibrary;
using ATSLibrary.Tariffs;
using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Program
    {
        private Random random = new Random();

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
                Console.WriteLine("Введите номер пункта:");

                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            ConnectTerminalToPorts(abonents,true);
                            break;
                        case 2:
                            ConnectTerminalToPorts(abonents,false);
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
                            PutMoney(abonents);
                            break;
                        case 7:
                            GetHistory(ats,abonents);
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

        private static void ChangeTariff(Abonent[] abonents)
        {
            Console.WriteLine("Введите порядковый номер абонента");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent currentAbonent = abonents[number];

            Console.WriteLine($"Ваш текущий тариф:{currentAbonent.Dogovor.Tariff.Name}" + Environment.NewLine);
            Console.WriteLine("Доступные тарифные планы: " + Environment.NewLine);

            //не заморачивался тут с рефлексией
            Tariff[] tariffs = new Tariff[3];
            tariffs[0] = new EasyTariff();
            tariffs[1] = new MediumTariff();
            tariffs[2] = new FullTariff();

            for (int i = 0; i < tariffs.Length; i++)
            {
                Console.WriteLine(tariffs[i].Name);
            }

            Console.WriteLine("Выберите номер нового тарифного плана: ");
            int current = Convert.ToInt32(Console.ReadLine());
            currentAbonent.Dogovor.ChangeTariff(tariffs[current]);
        }

        private static void GetHistory(Station ats,Abonent[] abonents)
        {
            Console.WriteLine("Введите порядковый номер абонента");
            int number = Convert.ToInt32(Console.ReadLine());
            Abonent currentAbonent = abonents[number];
            ats.ShowHistory(currentAbonent.Port.AbonentNumber);
        }

        private static void PutMoney(Abonent[] abonents)
        {
            throw new NotImplementedException();
        }

        private static void GetBalance(Abonent[] abonents)
        {
            Console.WriteLine("Позвоните по номеру 100 :)");
        }

        private static void MakeDial(Abonent[] abonents)
        {
            Console.WriteLine("Введите порядковый номер абонента");
            int number = Convert.ToInt32(Console.ReadLine());

            Abonent abonent = abonents[number];

            Console.WriteLine("Введите номер для звонка:");
            int dialNumber = Convert.ToInt32(Console.ReadLine());
            abonent.Terminal.Dial(dialNumber);
            Console.ReadKey();
            abonent.Terminal.FinishDial();
        }

        private static void ConnectTerminalToPorts(Abonent[] abonents, bool isConnect)
        {
            Console.WriteLine("Введите порядковый номер абонента");
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
        private static Abonent[] CreateDogovors(Station ats,int count)
        {
            Abonent[] abonents = new Abonent[count];

            for (int i = 0; i < count; i++)
            {
                Dogovor Dogovor = ats.CreateDogovor(new EasyTariff());
                Port Port = ats.GetMyPort(Dogovor);
                Terminal Phone = ats.GetPhone();
                Phone.Ringing += Phone_Ringing;
                abonents[i] = new Abonent(Dogovor, Port, Phone);
                Console.WriteLine($"{i} Создан абонент с номером: {Port.AbonentNumber} Тариф: {Dogovor.Tariff.Name} Модель: {Phone.Name}");
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
    }
}
