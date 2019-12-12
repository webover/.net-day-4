using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IntegerLargestDivisors
{
    class Program
    {
        private static int numbersToBeProcessed = 100000;

        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            int processorCount = Environment.ProcessorCount;            

            int nrOfBatches = numbersToBeProcessed / processorCount;

            List<LargestDivisor> processorsList = new List<LargestDivisor>(processorCount);
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < processorCount; i++)
            {
                var ap = new LargestDivisor(i, nrOfBatches);

                processorsList.Add(ap);

                var t = new Thread(ap.CalculateDivisor);

                threads.Add(t);

                t.Start();
            }

            Dictionary<int, int> listLargestDivisors = new Dictionary<int, int>();

            for (int i = 0; i < processorCount; i++)
            {
                threads[i].Join();
                listLargestDivisors.Add(processorsList[i].largestDivisors, processorsList[i].count);
            }

            int highestLongValue = listLargestDivisors.Max(l => l.Value);

            var listDivisors = from lld in listLargestDivisors where lld.Value == highestLongValue select lld.Key;

            stopwatch.Stop();

            Console.WriteLine($"Numbers with the largest divisor will be {String.Join(" and ", listDivisors)} having a number of {highestLongValue} divisors and processed in {stopwatch.Elapsed.TotalSeconds} seconds");
        }
    }
}
