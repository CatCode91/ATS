using ATS.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public class Company<T> where T : Dogovor
    {
        T[] ports;

        public Company(string name)
        {
            CompanyName = name;
        }

        public string CompanyName { get; }

        public Dogovor CreateDogovor(Tariff tariff)
        {
            return new Dogovor(10,tariff);
        }

    }
}
