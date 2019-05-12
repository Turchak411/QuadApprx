using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadApprx
{
    class Program
    {
        private static List<PointData> bounds;
        private static int iter;

        static void Main(string[] args)
        {
            bounds = new List<PointData>();
            iter = 0;

            double eps = 0.001;

            Input();
            Calculate(eps);
            Output();
            Console.ReadKey();
        }

        private static void Input()
        {
            Console.Write("===========\nГРАНИЦЫ:\nНижняя: ");
            double x1 = double.Parse(Console.ReadLine());
            Console.Write("Верхняя: ");
            double x3 = double.Parse(Console.ReadLine());
            Console.WriteLine("===========");

            //double x2 = (Math.Abs(x1) + Math.Abs(x3)) / 2;

            bounds.Add(new PointData(x1, Func(x1)));
            //bounds.Add(new PointData(x2, Func(x2)));
            bounds.Add(new PointData(x3, Func(x3)));
        }

        private static void Calculate(double eps)
        {
            double a0;
            double a1;
            double a2;

            double fAVG;
            double minX;

            do
            {
                // Calc avg x:
                double x2 = (Math.Abs(bounds[0]._x) + Math.Abs(bounds[1]._x)) / 2;
                bounds.Insert(1, new PointData() { _x = x2, _y = Func(x2) });

                a0 = bounds[0]._y;
                a1 = (bounds[1]._y - bounds[0]._y) / (bounds[1]._x - bounds[0]._x);
                a2 = 1 / (bounds[2]._x - bounds[1]._x) * ((bounds[2]._y - bounds[0]._y) / (bounds[2]._x - bounds[0]._x) - a1);

                int maxFuncIndex = GetMaxFuncIndex();

                bounds.RemoveAt(maxFuncIndex);

                double xAVG = (bounds[0]._x + bounds[1]._x) / 2 - a1 / (2 * a2);
                fAVG = Func(xAVG);

                // Calculate eps:
                minX = (bounds[0]._x > bounds[1]._x) ? bounds[1]._x : bounds[0]._x;

                iter++;
            }
            while (eps < Math.Abs((Func(minX) - fAVG) / fAVG));
        }

        private static int GetMaxFuncIndex()
        {
            int maxFuncIndex = 0;
            double maxFuncValue = bounds[maxFuncIndex]._y;

            for(int i = 1; i < bounds.Count; i++)
            {
                if(maxFuncValue < bounds[i]._y)
                {
                    maxFuncIndex = i;
                    maxFuncValue = bounds[i]._y;
                }
            }

            return maxFuncIndex;
        }

        private static void Output()
        {
            Console.WriteLine(String.Format("Конечный интервал:\n[ {0} {1} ]", bounds[0]._x, bounds[1]._x));
            Console.WriteLine("Количество итераций:" + iter);
        }

        private static double Func(double x)
        {
            return 2 * Math.Pow(x - 3, 2) + Math.Pow(Math.E, 0.5 * x);
        }
    }

    public struct PointData
    {
        public double _x;
        public double _y;

        public PointData(double x, double y)
        {
            _x = x;
            _y = y;
        }
    }
}
