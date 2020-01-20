using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Billing
    {
        private List<Call> journal = new List<Call>();

        internal void AddCall(Call call)
        {
            journal.Add(call);
            Console.WriteLine("Запись о звонке добавлена в журнал");
        }

        internal void PayCall(Dogovor dogovor, double amount)
        {
            dogovor.TakeFromBalance(amount);
        }

        internal List<Call> GetHistory(int number)
        {
            List<Call> calls = journal.Where(x => x.AbonentFrom == number).ToList();
            return calls;
        }
    }
}
