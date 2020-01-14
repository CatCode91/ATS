using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public struct Call
    {
        DateTime Date;
        TimeSpan Duration;
        CallType Type;

        public Call(DateTime date, TimeSpan duration, CallType type)
        {
            Date = date;
            Duration = duration;
            Type = type;
        }

        public enum CallType
        {
            Incoming,
            Outcoming
        }
    }
}
