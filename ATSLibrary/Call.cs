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
        }

        /// <summary>
        /// Дата начала разговора
        /// </summary>
        public DateTime StartDate
        {
            get;

        }

        /// <summary>
        /// Дата завершения разговора
        /// </summary>
        public DateTime FinishDate
        {
            get;
        }

        /// <summary>
        /// Продолжительность разговора
        /// </summary>
        public TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Тарифный план
        /// </summary>
        public Tariff Tariff
        {
            get;
        }

        /// <summary>
        /// Номер вызывающего абонента
        /// </summary>
        public int AbonentFrom
        {
            get;
        }

        /// <summary>
        /// Номер вызываемого абонента
        /// </summary>
        public int AbonentTo
        {
            get;
        }

        /// <summary>
        /// Стоимость звонка
        /// </summary>
        public decimal Amount
        {
            get;
        }
    }
}
