using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW9
{
    internal class DiscriminantException : Exception
    {
        public DiscriminantException(double discriminant) : base("Вещественных значений не найдено")
        {

        }
    }
}
