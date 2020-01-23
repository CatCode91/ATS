using System;
using ATSLibrary.Terminals;

namespace ATSLibrary
{
    //типа магаза. Пока будем тут хранить наши терминалы
    internal class Store
    {
        private Random _random = new Random();

        private string[] _phoneModels = 
            {
            "Alcatel", "BlackBerry", "iPhone", "Motorola", "Nokia", "Samsung", "Xiaomi"
        };

        //выдает рандомный телефон
        internal Phone GetPhone()
        {
            //выдает рандомный телефон абоненту
            int index = _random.Next(0, _phoneModels.Length);

            return new Phone(_phoneModels[index]);
        }
    }
}
