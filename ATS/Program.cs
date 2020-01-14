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
            Station<Dogovor> ats = new Station<Dogovor>("VELCOM");

            Dogovor dogovor = ats.CreateDogovor(new EasyTariff());

            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}
