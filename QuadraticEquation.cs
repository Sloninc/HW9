using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW9
{
    internal class QuadraticEquation
    {
        double _eps = 1E-10;
        double _discriminant;
        double _a, _b, _c;
        List<double> roots = new(); //Список для хранения корней уравнения
        public QuadraticEquation(int a=0, int b=0, int c=0)
        {
            _a = a;
            _b = b;
            _c = c;
            _discriminant = _b * _b - 4 * _a * _c;
            calcRoots();
        }
        public double Discriminant { get { return _discriminant; } }
        public List<double> Roots { get { return roots; } }


        void calcRoots()
        {
            if (_a == 0)
            {
                if(_b==0)
                    Console.WriteLine("Переменная может принимать любые значения");
                else
                {
                    double root1 = _c==0? 0:-_c / _b;
                    roots.Add(root1);
                }
            }
            else if (_discriminant > _eps)
            {
                double root1 = (_c==0)?0:(-_b + Math.Sqrt(_discriminant)) / (2 * _a);
                double root2 = (-_b - Math.Sqrt(_discriminant)) / (2 * _a);
                roots.Add(root1);
                roots.Add(root2);
            }
            else if (_discriminant < _eps&_discriminant>=0)
            {
                double root1 = _b==0?0:(-_b) / (2 * _a);
                roots.Add(root1);
            }
            else
            {
                throw new DiscriminantException(this);
            }
        }

    }
}
