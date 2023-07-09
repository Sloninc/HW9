using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW9
{
    internal class QuadraticEquation
    {
        double _discriminant;
        double _a, _b, _c;
        List<double> _roots = new();
        public QuadraticEquation(double a=0, double b=0, double c=0)
        {
            _a = a;
            _b = b;
            _c = c;
            _discriminant = _b * _b - 4 * _a * _c;
            _calcRoots();
        }
        public double Discriminant { get { return _discriminant; } }
        public List<double> Roots { get { return _roots; } }

        void _calcRoots()
        {
            if(_discriminant > 0)
            {
                double root1 = (-_b + Math.Sqrt(_discriminant)) / 2 * _a;
                double root2 = (-_b - Math.Sqrt(_discriminant)) / 2 * _a;
                _roots.Add(root1);
                _roots.Add(root2);
            }
            else if(_discriminant == 0)
            {
                double root1 = (-_b) / 2 * _a;
                _roots.Add(root1);
            }
            else
            {
                throw new DiscriminantException(_discriminant);
            }
        }

    }
}
