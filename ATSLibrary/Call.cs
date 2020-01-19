using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Call
    {
        public Call(DateTime startDate,DateTime finishDate, Tariff tariff, int fromAbonentNumber, int toAbonentNumber)
        {
            StartDate = startDate;
            FinishDate = finishDate;
            Duration = finishDate - startDate;
            Tariff = tariff;
            AbonentFrom = fromAbonentNumber;
            AbonentTo = toAbonentNumber;
        }

        public DateTime StartDate { get; private set; }
        public DateTime FinishDate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Tariff Tariff { get; private set; }
        public int AbonentFrom { get; private set; }
        public int AbonentTo { get; private set; }
    }
}
