using ATS.Tariffs;
using ATS.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    class Program
    {
        static void Main(string[] args)
        {
            Station ats = Station.GetInstance("VELCOM");

            Dogovor dogovor = ats.CreateDogovor(new EasyTariff());
            Dogovor dogovor1 = ats.CreateDogovor(new FullTariff());

            Phone phone = ats.GetPhone(dogovor);
            Phone phone2 = ats.GetPhone(dogovor);

            phone.Connect();

            Console.ReadKey();
        }
    }
}
