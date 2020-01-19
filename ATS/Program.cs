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
            CreateDogovors(ats,10);

            Console.ReadLine();
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
                Console.WriteLine($"Создан абонент с номером: {Port.AbonentNumber}, тариф: {Dogovor.Tariff.Name}, модель телефона: {Phone.Name}");
            }

            return abonents;
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
