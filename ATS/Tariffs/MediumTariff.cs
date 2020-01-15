using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Tariffs
{
    public class MediumTariff : Tariff
    {
        public override string Name => "Народный!";

        public override double Rate => 0.7;
    }
}
