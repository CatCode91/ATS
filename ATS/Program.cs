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
            //cоздаем объект станции
            Station ats = new Station("VELCOM");
            //выбираем тариф
            Tariff tariff = new EasyTariff();
            //Заключаем договор согласно выбранного тарифа
            Dogovor dogovor = ats.CreateDogovor(tariff);
            //получаем порт согласно договора
            Port port = ats.GetPort(dogovor);

        
            Console.ReadKey();
        }
    }
}
