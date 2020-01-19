using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Billing
    {
        private List<Call> journal = new List<Call>();

        public void AddCall(Call call)
        {
            journal.Add(call);
        }

    }
}
