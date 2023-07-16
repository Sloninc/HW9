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
        //private QuadraticEquation equation;
        public DiscriminantException(QuadraticEquation qEquit):base("Вещественных значений не найдено")
        {
            Data.Add("Discriminant", qEquit.Discriminant);
        }
      
    }
}
