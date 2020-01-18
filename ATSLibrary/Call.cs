using ATSLibrary.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    internal class Call
    {
        DateTime Date;
        TimeSpan Duration;
        Tariff Tariff;
        int IncomingNumber;
        int OutComingNumber;

        public Call(DateTime date, TimeSpan duration, Tariff tariff,int incomingNumber, int outComingNumber)
        {
            Date = date;
            Duration = duration;
            Tariff = tariff;
            IncomingNumber = incomingNumber;
            OutComingNumber = outComingNumber;
        } 
    }

}
