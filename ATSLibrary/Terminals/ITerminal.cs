using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSLibrary.Terminals
{
    public interface ITerminal
    {
        /// <summary>
        /// Набрать номер
        /// </summary>
        /// <param name="number"></param>
        void StartDial(int number);

        /// <summary>
        /// Завершить звонок
        /// </summary>
        void FinishDial();

        /// <summary>
        /// Подключить к порту
        /// </summary>
        /// <param name="port"></param>
        void ConnectPort(Port port);

        /// <summary>
        /// Отключиться от порта
        /// </summary>
        void DisconnectPort();
        
        /// <summary>
        /// Поднять трубку
        /// </summary>
        void AnswerCall();

        /// <summary>
        /// Занято
        /// </summary>
        void SendBusy();
    }
}
