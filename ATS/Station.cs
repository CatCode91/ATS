using ATS.Tariffs;
using ATS.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Station
    {
        //количество портов на станции
        private Port[] ports = new Port[100];
        //список заключенных договоров
        private List<Dogovor> dogovors = new List<Dogovor>();
        private static Station instance;

        private Station(string name)
        {
            CompanyName = name;
        }

        public static Station GetInstance(string name)
        {
            if (instance == null)
                instance = new Station(name);
            return instance;
        }

        public string CompanyName { get; }

        public Dogovor CreateDogovor(Tariff tariff)
        {
            int number = dogovors.Count() + 1;

            Dogovor dogovor = new Dogovor(number, tariff);
            dogovors.Add(dogovor);
            return new Dogovor(number, tariff);
        }

        public Phone GetPhone(Dogovor dogovor)
        {
            return new Phone(dogovor);
        }

        private void InitializePorts()
        {
            for (int i = 0; i <= ports.Length; i++)
            {
                ports[i] = new Port();
                ports[i].StateChanging += StateChanging;
            }
        }

        private void StateChanging(Port sender, PortEventArgs e)
        {
            Console.WriteLine($"Изменилось состояние порта: {sender.Number}");
        }

      

    }
}
