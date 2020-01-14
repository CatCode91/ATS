using ATS.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Dogovor
    {
        private DateTime _dateOfCreation;
        private Tariff _tariff;
        private Port _port;

        public Dogovor(int dogovorNumber,Tariff tariff)
        {
            DogovorNumber = dogovorNumber;
            _tariff = tariff;
            _dateOfCreation = DateTime.Today;

        }
    
        public int DogovorNumber { get;}
        public DateTime DateOfCreation => _dateOfCreation;
        public Tariff Tariff => _tariff;
        public Port Port => _port;

        public void ChangeTariff(Tariff tariff)
        {
            var today = DateTime.Today;
            if (_dateOfCreation > today.AddMonths(-1))
            {
                _tariff = tariff;
                Console.WriteLine($"Поздравляем! Ваш тариф изменен на - {tariff.Name}");
            }

            else
            {
                Console.WriteLine("К сожалению, тариф можно изменить только раз в месяц");
            }
            

        }
    }
}
