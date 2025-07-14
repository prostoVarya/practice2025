using System.Threading;
namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
        {
            double result = 0.0;

            double length = (b - a) / threadsnumber;

            object locker = new object();
            var barrier = new Barrier(threadsnumber + 1);

            for (int i = 0; i < threadsnumber; i++)
            {
                double begin = a + i * length;
                double end = (i == threadsnumber - 1) ? b : begin + length;
               
                Thread thread = new Thread(_ =>
                {
                    lock (locker)
                    {
                        result += TrapezoidMethod(begin, end, function, step);
                    }

                    barrier.SignalAndWait();
                });

                thread.Start();
            }

            barrier.SignalAndWait();
            barrier.Dispose();

            return result;
        }
        public static double TrapezoidMethod(double a, double b, Func<double, double> function, double step)
        {
            double result = 0.0;

            for (double x = a; x < b; x += step) result += (b < x + step) ? (function(x) + function(b)) / 2 * (b - x) 
                    : (function(x) + function(x + step)) / 2 * step;
            return result;
        }
    }
}