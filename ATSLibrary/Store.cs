using ATSLibrary.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal Phone GetPhone()
        {
            //выдает рандомный телефон абоненту
            int index = _random.Next(0, _phoneModels.Length);

            return new Phone(_phoneModels[index]);
        }



    }
}
