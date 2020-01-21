using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Dogovor
    {
        private double _debt;

        internal Dogovor(int dogovorNumber, Tariff tariff)
        {
            Balance = 0.00;
            DogovorNumber = dogovorNumber;
            Tariff = tariff;
            DateOfCreation = DateTime.Today;
        }

        public int DogovorNumber
        {
            get;
        } 
        public DateTime DateOfCreation
        {
            get;
            private set;
        }
        public Tariff Tariff { get; private set; }
        internal double Balance
        {
            get;
            private set;
        }

        internal double Debt
        {
            get
            {
                return _debt;
            }

            set
            {
                _debt = value;

                if (_debt < 0 )
                {
                    Balance += Math.Abs(_debt);
                }
            }
        }

        public void ChangeTariff(Tariff tariff)
        {
            var today = DateTime.Today;

            if (today.AddMonths(-1) >= DateOfCreation)
            {
                if (Tariff == tariff)
                {
                    Console.WriteLine($"У вас уже установлен тариф - {tariff.Name}");
                    return;
                }

                Tariff = tariff;
                Console.WriteLine($"Поздравляем! Ваш тариф изменен на - {tariff.Name}");
            }

            else
            {
                Console.WriteLine($"К сожалению, тариф можно изменить только раз в месяц (не раньше {_dateOfCreation.AddMonths(1).ToShortDateString()})");
            }

        }

        internal void PayBills(double sum)
        {
            Debt -= sum;
            Console.WriteLine($"На счет внесено {sum} BYN. Ваш баланс составляет {Balance} BYN");
        }
    }
}
