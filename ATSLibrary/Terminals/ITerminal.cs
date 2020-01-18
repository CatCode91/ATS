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
        /// Находится ли трубка в снятом положении 
        /// </summary>
        bool IsTubeUp { get; }

        /// <summary>
        /// Наименование устройства
        /// </summary>
        string Name { get; }

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
        /// Подтвердить ответ
        /// </summary>
        void AcceptCall();

        /// <summary>
        /// Занято
        /// </summary>
        void SendBusy();
    }
}
