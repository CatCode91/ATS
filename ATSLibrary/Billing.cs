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
        //список всех завершенных вызовов на станции
        private List<Call> _journal = new List<Call>();

        //день расчета задолженности
        internal int LastPayDay => 22;

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
        /// Возвращает список совершенных звонков 
        /// </summary>
        /// <param name="number">Номер телефона</param>
        /// <returns></returns>
        internal List<Call> GetHistory(int number)
        {
            List<Call> calls = _journal.Where((x => x.AbonentFrom == number || x.AbonentTo == number)).ToList();

            return calls;
        }

        /// <summary>
        /// Возвращает возможность совершения звонка, учитывая финансовую ситуацию
        /// </summary>
        internal bool IsBillsPaid(double debt)
        {
            return (debt >= 0) ? true : false;
        }

        /// <summary>
        /// Подсчет суммы потраченной на звонок
        /// </summary>
        /// <param name="tariff">Текущий тариф абонента</param>
        /// <param name="duration">Длительность звонка</param>
        /// <returns></returns>
        internal double GetCallPrice(Tariff tariff,TimeSpan duration)
        {
            return duration.TotalSeconds * tariff.Rate;
        }

        /// <summary>
        /// Посчитать счет по абоненту за прошлый месяц
        /// </summary>
        /// <returns></returns>
        internal double GetBillLastMonth(int dogovorNumber,DateTime firstDate,DateTime lastDate)
       {
            //фильтруем журнал по дате, затем по номеру договора
            var dateFilter = _journal.Where(x => (x.StartDate >= firstDate) & (x.StartDate <= lastDate));
            var abonentFilter = dateFilter.Where(x => x.DogovorNumber == dogovorNumber);

            double amount = 0;
            
            //подбиваем сумму за период
            foreach (var i in abonentFilter)
            {
                amount += i.Amount;
            }

            return amount;
        }
       
        /// <summary>
        /// Подсчитать задолженность по всем абонентам за прошлый месяц
        /// </summary>
        /// <param name="dogovors"></param>
        internal void CountDebtsForAbonents(IEnumerable<Dogovor> dogovors)
        {
            //прошлый месяц
            DateTime date = DateTime.Now.AddMonths(-1);

            //дата начала прошлого месяца
            DateTime firstDate = new DateTime(date.Year, date.Month, 1);

            //дата конца прошлого месяца
            DateTime lastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

            foreach (Dogovor dogovor in dogovors)
            {
                if (dogovor.LastDateDebtCounted.Month == DateTime.Now.Month)
                {
                    Console.WriteLine("Договор заключен в текущем месяце, либо в этом месяце уже производился расчет задолженности!");
                    continue;
                }

                double summ = GetBillLastMonth(dogovor.DogovorNumber, firstDate,lastDate);
                dogovor.SetDebt(summ);
                Console.WriteLine($"По договору № {dogovor.DogovorNumber } оказано услуг на сумму: {summ} BYN");
            }
        }
    }
}
