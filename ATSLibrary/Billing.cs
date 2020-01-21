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

        public Billing()
        {
             LastDayPays = new DateTime(DateTime.Now.Year, DateTime.Now.Month, _dayPayBills);
        }

        internal DateTime LastDayPays { get; private set; }

        internal void AddCall(Call call)
        {
            journal.Add(call);
            Console.WriteLine("Запись о звонке добавлена в журнал");
        }

        internal void TakeCallPrice(Dogovor dogovor, double amount)
        {
            dogovor.TakeFromBalance(amount);
        }

        internal List<Call> GetHistory(int number)
        {
            List<Call> calls = journal.Where((x => (x.AbonentFrom == number) & (x.AbonentTo == number))).ToList();

            return calls;
        }

        internal bool IsBillsPaid(Dogovor dogovor)
        {
            //если есть деньги на счету, звонить можно
            if (dogovor.Balance > 0)
            {
                return true;
            }

            //если долги погашены до контрольного дня, разрешаем звонить
            if ((dogovor.DateOfLastPay >= LastDayPays.AddMonths(-1)))
            {
                return true;
            }

            else
            {
                Console.WriteLine($"Оплатите предыдущий счет! Оплата до {LastDayPays}");
                Console.WriteLine($"Последний платеж совершен {dogovor.DateOfLastPay}");
                return false;
            }
        }
    }
}
