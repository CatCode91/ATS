using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Tariffs
{
    public class EasyTariff : Tariff
    {
        public override string Name => @"""Легкий""";
        public override double Rate => 0.01;
    }
}
