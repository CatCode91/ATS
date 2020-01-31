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
        internal Dogovor(int dogovorNumber, Tariff tariff)
        {
            Balance = 0.00;
            DogovorNumber = dogovorNumber;
            Tariff = tariff;
            DateOfCreation = DateTime.Today;
            DateChangeTariff = DateTime.Today;
        }

        /// <summary>
        /// Номер договора
        /// </summary>
        public int DogovorNumber
        {
            get;
        } 

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public DateTime DateOfCreation
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Тарифный план
        /// </summary>
        public Tariff Tariff
        {
            get;
            private set;
        }

        /// <summary>
        /// Денежные средства на балансе
        /// </summary>
        internal double Balance
        {
            get;
            private set;
        } = 0;

        //должен быть либо 0 либо отрицательным числом (задолженность)
        internal double Debt
        {
            get;
            private set;
        } = 0;
        
        /// <summary>
        /// Дата последнего изменения тарифного плана
        /// </summary>
        internal DateTime DateChangeTariff
        {
            get;
            private set;
        }

        /// <summary>
        /// Дата, когда последний раз по счету устанавливалась задолженность
        /// </summary>
        internal DateTime LastDateDebtCounted
        {
            get;
            private set;
        } = DateTime.Now;

        /// <summary>
        /// Сменить тарифный план
        /// </summary>
        /// <param name="tariff">Тарифный план</param>
        public void ChangeTariff(Tariff tariff)
        {
            if (DateTime.Today.AddMonths(-1) >= DateChangeTariff)
            {
                if (Tariff == tariff)
                {
                    Console.WriteLine($"У вас уже установлен тариф - {tariff.Name}");
                    return;
                }

                Tariff = tariff;
                Console.WriteLine($"Поздравляем! Ваш тариф изменен на - {tariff.Name}");
                DateChangeTariff = DateTime.Now;
            }

            else
            {
                Console.WriteLine($"К сожалению, тариф можно изменить только раз в месяц (не раньше {DateChangeTariff.AddMonths(1).ToShortDateString()})");
            }

        }

        /// <summary>
        /// Пополнение счета
        /// </summary>
        /// <param name="sum"></param>
        internal void PayBills(double sum)
        {
            Debt += sum;

            //если внесено с запасом(долг больше 0), то остаток идет на баланс
            if (Debt >= 0)
            {
                Balance += Debt;
                Debt = 0;
            }

            //отображаем баланс с учетом задолженности
            Console.WriteLine($"На счет внесено {sum} BYN. Ваш баланс составляет {Balance+Debt} BYN");
        }

        /// <summary>
        /// Установить задолженность по договору
        /// </summary>
        /// <param name="sum"></param>
        internal void SetDebt(double sum)
        {
            Debt -= sum;

            //если в момент подсчета на балансе есть деньги, списываем их на покрытие долга
            if (Balance > 0)
            {
                Balance += Debt;

                if (Balance <= 0)
                {
                    Debt = Balance;
                    Balance = 0;
                }

                else
                {
                    Debt = 0;
                }
            }

            LastDateDebtCounted = DateTime.Now;
        }
    }
}
