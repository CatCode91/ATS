using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Billing
    {
        private List<Call> _journal = new List<Call>();

        //день расчета задолженности
        internal int LastPayDay { get; } = 22;

        /// <summary>
        /// Добавляет запись в историю звонков
        /// </summary>
        /// <param name="call"></param>
        internal void AddCallToJournal(Call call)
        {
            _journal.Add(call);
            Console.WriteLine("Запись о звонке добавлена в журнал");
        }

        /// <summary>
        /// Возвращает историю по номеру телефона
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        internal List<Call> GetHistory(int number)
        {
            List<Call> calls = _journal.Where((x => x.AbonentFrom == number || x.AbonentTo == number)).ToList();

            return calls;
        }

        /// <summary>
        /// Возвращает возможность совершения звонка, учитывая финансовую ситуацию
        /// </summary>
        /// <param name="dogovor"></param>
        /// <returns></returns>
        internal bool IsBillsPaid(Dogovor dogovor)
        {
            return (dogovor.Debt >= 0) ? true : false;
        }

        /// <summary>
        /// Подсчет суммы, потраченной на звонок
        /// </summary>
        /// <param name="tariff">Текущий тариф абонента</param>
        /// <param name="duration">Длительность звонка</param>
        /// <returns></returns>
        internal double GetCurrentCallPrice(Tariff tariff,TimeSpan duration)
        {
            return tariff.Rate * duration.TotalSeconds;
        }

        /// <summary>
        /// Посчитать счет по абоненту за прошлый месяц
        /// </summary>
        /// <param name="dogovor"></param>
        /// <param name="port"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        internal double CountBillLastMonth(Dogovor dogovor)
       {
            //дата начала прошлого месяца
            DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month -1 , 1);
            
            //дата конца прошлого месяца
            DateTime lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            //фильтруем журнал по дате, затем по номеру договора
            var dateFilter = _journal.Where(x => (x.StartDate >= firstDate) & (x.StartDate <= lastDate));
            var abonentFilter = dateFilter.Where(x => x.Dogovor.DogovorNumber == dogovor.DogovorNumber);

            double amount = 0;
            
            //подбиваем сумму за период
            foreach (var i in abonentFilter)
            {
                amount += i.Amount;
            }

            return amount;
        }
       
        /// <summary>
        /// Подсчитать задолженность по всем абонентам
        /// </summary>
        /// <param name="dogovors"></param>
        internal void CountDebtsForAbonents(IEnumerable<Dogovor> dogovors)
        {
            foreach (Dogovor dogovor in dogovors)
            {
                double summ = CountBillLastMonth(dogovor);
                dogovor.SetDebt(summ);
            }
        }
    }
}
