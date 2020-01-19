using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Tariffs
{
    public class Medium : Tariff
    {
        public override string Name => @"""Народный""";
        public override double Rate => 1;
    }
}
