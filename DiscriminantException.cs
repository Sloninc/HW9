using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW9
{
    internal class DiscriminantException : Exception
    {
        private IDictionary<string, string>? _date;
        public DiscriminantException(QuadraticEquation qEquit):base("Вещественных значений не найдено")
        {
            Data= new Dictionary<string,string>(){ { "Discriminant", qEquit.Discriminant.ToString() } };
        }
        public new IDictionary<string,string> Data
        {
            get
            {
                    return _date;
            }
            private set
            {
                _date = value;
            }
        }
    }
}
