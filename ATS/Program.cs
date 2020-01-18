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
        static void Main(string[] args)
        {
            Console.WriteLine();

            //Создаем объект телефонной станции
            Station ats = new Station("VELCOM");

            //Создаем перового абонента, регистрируем ему порт согласно договора и выдаем терминал        
            Dogovor firstDogovor = ats.CreateDogovor(new EasyTariff());
            Port firstPort = ats.GetMyPort(firstDogovor);
            Terminal firstPhone = ats.GetPhone(PhoneModels.Alcatel);
            firstPhone.Ringing += Phone_Ringing;
            firstPhone.ConnectPort(firstPort);
            Console.WriteLine($"Номер телефона абонента: {firstPort.AbonentNumber} модель: {firstPhone.Name}");
            Console.WriteLine();

            //Создаем второго абонента
            Console.WriteLine();
            Dogovor secondDogovor = ats.CreateDogovor(new FullTariff());
            Port secondPort = ats.GetMyPort(secondDogovor);
            Terminal secondPhone = ats.GetPhone(PhoneModels.Nokia);
            secondPhone.Ringing += Phone_Ringing;
            secondPhone.ConnectPort(secondPort);
            Console.WriteLine($"Номер телефона абонента: {secondPort.AbonentNumber} модель: {secondPhone.Name}");
            Console.WriteLine();

            //Совершаем звонок
            firstPhone.StartDial(29001);
            Console.ReadKey();

            secondPhone.StartDial(29000);
            //Console.ReadKey();

        }

        private static void Phone_Ringing(ITerminal sender, TerminalEventArgs e)
        {
            Console.WriteLine($"{sender.Name}: {e.Message}");
            Console.WriteLine($"Ответить - 1, Сбросить - 2");

            var command = int.Parse(Console.ReadLine());

           switch (command)
            {
                case 1:
                    sender.SendAcceptCall(true);
                    break;
                case 2:
                    sender.SendAcceptCall(false);
                    break;
            }
        }

    }
}
