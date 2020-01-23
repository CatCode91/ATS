using System;
using ATSLibrary.Tariffs;

namespace ATSLibrary
{
    public class Call
    {
        public Call(int dogovorNumber, Tariff tariff, DateTime startDate,DateTime finishDate, int fromAbonentNumber, int toAbonentNumber, decimal amount)
        {
            DogovorNumber = dogovorNumber;
            StartDate = startDate;
            FinishDate = finishDate;
            Duration = finishDate - startDate;
            AbonentFrom = fromAbonentNumber;
            AbonentTo = toAbonentNumber;
            Tariff = tariff;
            Amount = amount;
        }

        /// <summary>
        /// Номер договора вызываемого абонента
        /// </summary>
        public int DogovorNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// Дата начала разговора
        /// </summary>
        public DateTime StartDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Дата завершения разговора
        /// </summary>
        public DateTime FinishDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Продолжительность разговора
        /// </summary>
        public TimeSpan Duration
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
        /// Номер вызывающего абонента
        /// </summary>
        public int AbonentFrom
        {
            get;
            private set;
        }

        /// <summary>
        /// Номер вызываемого абонента
        /// </summary>
        public int AbonentTo
        {
            get;
            private set;
        }

        /// <summary>
        /// Стоимость звонка
        /// </summary>
        public decimal Amount
        {
            get;
            private set;
        }
    }
}
