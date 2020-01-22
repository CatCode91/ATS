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
        private DateTime _lastDateDebtCounted = DateTime.Now;

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

        //должен быть либо 0 либо отрицательным числом (задолженность)
        internal double Debt { get; private set; } = 0;

        //смена тарифного плана
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
                Console.WriteLine($"К сожалению, тариф можно изменить только раз в месяц (не раньше {DateOfCreation.AddMonths(1).ToShortDateString()})");
            }

        }

        //оплата долгов
        internal void PayBills(double sum)
        {
            Debt += sum;

            //если внесено с запасом(долг больше 0), то остаток идет на баланс
            
            if (Debt > 0)
            {
                Balance += Math.Abs(Debt);
                Debt = 0;
            }

            Console.WriteLine($"На счет внесено {sum} BYN. Ваш баланс составляет {Balance + Debt} BYN");
        }

        //установка размера задолженности
        internal void SetDebt(double sum)
        {
            if (_lastDateDebtCounted.Month == DateTime.Now.Month)
            {
                Console.WriteLine("Договор заключен в текущем месяце, либо уже производился расчет задолженности!");
                return;
            }

            Debt = -sum;

            //если в момент подсчета на балансе есть деньги, списываем их на покрытие долга
            if (Balance > 0)
            {
                Debt += Balance;
                Balance = 0;
            }

            _lastDateDebtCounted = DateTime.Now;
            Console.WriteLine($"Рассчет задолженности произведен, размер долга состалвяет! {Debt} BYN") ;
        }
    }
}
