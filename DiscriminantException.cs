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
        public DiscriminantException(QuadraticEquation qEquit):base("Вещественных значений не найдено")
        {
            Data[nameof(qEquit.Discriminant)] = qEquit.Discriminant;
        }
    }
}
