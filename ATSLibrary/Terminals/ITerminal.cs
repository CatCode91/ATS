﻿namespace ATSLibrary
{
    public interface ITerminal
    {
        /// <summary>
        /// Имя устройства
        /// </summary>
        string Name
        {
            get;
        }

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
        /// Набрать номер
        /// </summary>
        /// <param name="number"></param>
        void Dial(int number);

        /// <summary>
        /// Завершить звонок
        /// </summary>
        void FinishDial();

        /// <summary>
        /// Подтвердить или отклонить входящий вызов
        /// </summary>
        void SendAcceptCall(bool accepted);
    }
}
