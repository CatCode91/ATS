using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Billing
    {
        private readonly int _dayPayBills = 25;

        private List<Call> journal = new List<Call>();

        internal void AddCallToJournal(Call call)
        {
            journal.Add(call);
            Console.WriteLine("Запись о звонке добавлена в журнал");
        }

        internal List<Call> GetHistory(int number)
        {
            List<Call> calls = journal.Where((x => x.AbonentFrom == number || x.AbonentTo == number)).ToList();

            return calls;
        }

        internal bool IsBillsPaid(Dogovor dogovor)
        {
            return (dogovor.Debt > 0) ? false : true;
        }

        //подбить счет по абоненту
        internal double CountBill(Dogovor dogovor, Port port, DateTime date)
        {
            DateTime firstDate = new DateTime(date.Year, date.Month -1 , 1);

            DateTime lastDate = new DateTime(date.Year, date.Month + 1, 1).AddDays(-1);

            var dateFilter = journal.Where(x => (x.StartDate >= firstDate) & (x.StartDate <= lastDate));
            var abonentFilter = dateFilter.Where(x => (x.AbonentFrom == port.AbonentNumber) & (x.Dogovor.DogovorNumber == dogovor.DogovorNumber));

            double amount = 0;

            foreach (var i in abonentFilter)
            {
                amount += i.Amount;
            }

            return amount;
        }
    }
}
