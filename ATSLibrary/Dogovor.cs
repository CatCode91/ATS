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
        private DateTime _dateOfCreation;
        private DateTime _dateOfLastPay;
        private Tariff _tariff;
        private double _balance = 0.00;

        internal Dogovor(int dogovorNumber, Tariff tariff)
        {
            DogovorNumber = dogovorNumber;
            _tariff = tariff;
            _dateOfCreation = DateTime.Today;
            _dateOfLastPay = DateTime.Now;
        }

        public int DogovorNumber { get; } 
        public DateTime DateOfCreation => _dateOfCreation;
        internal DateTime DateOfLastPay => _dateOfLastPay;
        public Tariff Tariff => _tariff; 
        internal double Balance => _balance;

        public void ChangeTariff(Tariff tariff)
        {
            var today = DateTime.Today;

            if (today.AddMonths(-1) >= DateOfCreation)
            {
                if (_tariff == tariff)
                {
                    Console.WriteLine($"У вас уже установлен тариф - {tariff.Name}");
                    return;
                }

                _tariff = tariff;
                Console.WriteLine($"Поздравляем! Ваш тариф изменен на - {tariff.Name}");
            }

            else
            {
                Console.WriteLine($"К сожалению, тариф можно изменить только раз в месяц (не раньше {_dateOfCreation.AddMonths(1).ToShortDateString()})");
            }

        }
        internal void TakeFromBalance(double sum)
        {
            _balance -= sum;
        }
        internal void PayBills(double sum)
        {
            _balance += sum;

            if (_balance >= 0)
            {
                _dateOfLastPay = DateTime.Now;
            }

            Console.WriteLine($"На счет внесено {sum} BYN. Ваш баланс составляет {Balance} BYN");
        }
    }
}
