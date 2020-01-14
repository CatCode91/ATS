using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS
{
    public struct Call
    {     
        TimeSpan Duration;
        CallType Type;

        public Call(TimeSpan duration, CallType type)
        {
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
