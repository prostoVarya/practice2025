using System.Diagnostics;
using ScottPlot;
using task14;
namespace task15
{
    public class Program
    {
        static Func<double, double> SIN = (double x) => Math.Sin(x);
        static void Main()
        {

            var steps = new List<double> { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };

            double accuracy = 1e-4;
            double optStep = 0;
            double exactIntegralValue = 0;

            foreach (double step in steps)
            {
                double integral = DefiniteIntegral.TrapezoidMethod(-100, 100, SIN, step);

                double currAccuracy = Math.Abs(exactIntegralValue - integral);
                optStep = currAccuracy < accuracy ? Math.Max(step, optStep) : optStep;
               
            }


            int countOfReplays = 10;
            Dictionary<int, double> timeAndCountOfThreads = new Dictionary<int, double>();
            for (int countOfThreads = 1; countOfThreads < 32; countOfThreads++)
            {
                double allTime = 0;
                
                for (int i = 0; i < countOfReplays; i++)
                {
                    var timeChecker = new Stopwatch();
                    timeChecker.Start();

                    double integral = DefiniteIntegral.Solve(-100, 100, SIN, optStep, countOfThreads);

                    timeChecker.Stop();
                    allTime += timeChecker.Elapsed.TotalNanoseconds;
                    
                }
                
                double currResultTime = allTime / countOfReplays;

                timeAndCountOfThreads[countOfThreads] = currResultTime;
            }


            double optTimeWithThreads = timeAndCountOfThreads.Values.Min();
            int optCountOfThreads = timeAndCountOfThreads.Keys.Where(key => timeAndCountOfThreads[key] == optTimeWithThreads).Min();

            double[] oyValues = timeAndCountOfThreads.Values.ToArray();
            int[] oxValues = timeAndCountOfThreads.Keys.ToArray();

            ScottPlot.Plot plot = new ScottPlot.Plot();


            var scatter = plot.Add.Scatter(oxValues, oyValues);

       
            plot.YLabel("Время вычисления, нс");
            plot.XLabel("Количество потоков");

  
            plot.SavePng("grafic.png", 500, 500);


            double allTimeWithoutThreads = 0;

            for (int i = 0; i < countOfReplays; i++)
            {
                var timeChecker = new Stopwatch();
                timeChecker.Start();

                double integral = DefiniteIntegral.TrapezoidMethod(-100, 100, SIN, optStep);

                timeChecker.Stop();
                allTimeWithoutThreads += timeChecker.Elapsed.TotalNanoseconds;

            }

            double avergeTimeWithoutThreads = allTimeWithoutThreads / countOfReplays;

            double procent = Math.Abs(avergeTimeWithoutThreads - optTimeWithThreads) / avergeTimeWithoutThreads * 100;

            var sw = new StreamWriter("text_file.txt");

            string resultStr = $"Оптимальный шаг: {optStep}\n" +
                $"Оптимальное количество потоков: {optCountOfThreads}\n" +
                $"Оптимальное время при работе с потоками: {optTimeWithThreads}\n" +
                $"Среднее время при работе без потоков {avergeTimeWithoutThreads}\n" +
                $"Разница в процентах {procent}";

            sw.WriteLine(resultStr);
            Console.WriteLine(resultStr);

            sw.Close();

        }
    }
}