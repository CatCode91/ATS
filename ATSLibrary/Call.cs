using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public class Call
    {
        public Call(Tariff tariff, DateTime startDate,DateTime finishDate, int fromAbonentNumber, int toAbonentNumber, double amount)
        {
            StartDate = startDate;
            FinishDate = finishDate;
            Duration = finishDate - startDate;
            AbonentFrom = fromAbonentNumber;
            AbonentTo = toAbonentNumber;
            Tariff = tariff;
            Amount = amount;
        }

        public DateTime StartDate { get; private set; }
        public DateTime FinishDate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Tariff Tariff { get; private set; }
        public int AbonentFrom { get; private set; }
        public int AbonentTo { get; private set; }
        public double Amount { get; private set; }
    }
}
