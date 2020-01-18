using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary
{
    public enum PortStatus
    {
        /// <summary>
        /// порт не имеет номера и устройства
        /// </summary>
        Free,
        /// <summary>
        ///порту присвоен абонентский номер, но не подключен терминал
        /// </summary>
        Disconnected,
        /// <summary>
        ///присвоен абонентский номер и подключен терминал
        /// </summary>
        Connected,
        /// <summary>
        /// порт занят (поступил входящий или исходящий вызов)
        /// </summary>
        Busy,      
    }
}
