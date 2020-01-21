using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Tariffs
{
    public class FullTariff : Tariff
    {
        public override string Name => @"""Бизнес""";
        public override double Rate => 0.1;
    }
}
