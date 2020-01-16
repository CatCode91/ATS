using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Tariffs
{
    public abstract class Tariff
    {
        public abstract string Name { get; }
        public abstract double Rate { get; }
    }
}
