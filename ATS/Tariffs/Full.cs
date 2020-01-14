using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Tariffs
{
    public class Full : Tariff
    {
        public override string Name => "Для серьезных пацанов!";

        public override double Rate => 1.4;
    }
}
