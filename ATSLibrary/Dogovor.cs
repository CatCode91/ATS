﻿using ATSLibrary.Tariffs;
using System;

namespace ATSLibrary
{
    public class Dogovor
    {
        private decimal _debt;

        internal Dogovor(int dogovorNumber, Tariff tariff)
        {
            Balance = 0.00M;
            Debt = 0.00M;
            DogovorNumber = dogovorNumber;
            Tariff = tariff;
            DateOfCreation = DateTime.Today;
            DateChangeTariff = DateTime.Today;
            LastDateDebtCounted = DateTime.Today;
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
        internal decimal Balance
        {
            get;
            private set;
        }

        //должен быть либо 0 либо отрицательным числом (задолженность)
        //если больше 0, остаток переносится на баланс
        internal decimal Debt
        {
            get
            {
                return _debt;
            }

            set
            {
                _debt = value;

                if (_debt >= 0)
                {
                    Balance += _debt;
                    _debt = 0;
                }
            }
        }

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
        }

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
        internal void PayBills(decimal sum)
        {
            Debt += sum;

            //отображаем баланс и задолженность
            Console.WriteLine($"На счет внесено {sum} BYN. Ваш баланс составляет {Balance} BYN");
            Console.WriteLine($"Задолженность составляет: {Debt} BYN");
        }

        /// <summary>
        /// Установить задолженность по договору
        /// </summary>
        /// <param name="sum"></param>
        internal void SetDebt(decimal sum)
        {
            Debt -= sum;

            //если в момент подсчета на балансе есть деньги, списываем все их на покрытие долга
            if (Balance > 0)
            {
                Balance += Debt;

                //Если после списания баланс отрицательный превращаем его в долг.
                if (Balance <= 0)
                {
                    Debt = Balance;
                    Balance = 0;
                }

                //а если нет, то и долга нет и баланс положительный :)
                else
                {
                    Debt = 0;
                }

                LastDateDebtCounted = DateTime.Now;
            }
        }
    }
}
