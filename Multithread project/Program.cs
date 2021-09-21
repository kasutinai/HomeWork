using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Multithread_project
{
    class Program
    {
        private static object _lock = new();
        static void Main(string[] args)
        {
            Calculate(100000);
            Calculate(1000000);
            Calculate(10000000);
        }

        private static void Calculate(int arraySize)
        {
            var sum = 0;
            var array = CreateArray(arraySize);
            Stopwatch sw = new Stopwatch();

            Console.WriteLine($"{arraySize} records");
            sw.Start();
            sum = BasicCalculation(array);
            sw.Stop();
            Console.WriteLine($"Basic calculation: {sw.Elapsed.TotalMilliseconds}, sum = {sum}");

            sw.Restart();
            sum = ThreadCalculation(array);
            sw.Stop();
            Console.WriteLine($"Thread calculation: {sw.Elapsed.TotalMilliseconds}, sum = {sum}");

            sw.Restart();
            sum = PLinqCalculation(array);
            sw.Stop();
            Console.WriteLine($"PLinq calculation: {sw.Elapsed.TotalMilliseconds}, sum = {sum}");
        }
        
        private static int[] CreateArray(int size)
        {
            int[]   array = new int[size];
            var     rndm  = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rndm.Next(9);
            }

            return array;
        }

        private static int BasicCalculation(int[] _array)
        {
            return _array.Sum();
        }

        private static void ArraySum(int[] _array, ref int sum)
        {
            var subSum = BasicCalculation(_array);
            lock (_lock)
            {
                sum += subSum;
            }
        }

        private static int ThreadCalculation(int[] _array)
        {
            var threads = Environment.ProcessorCount;
            var threadPool = new List<Thread>(threads);
            var partSize = _array.Length / threads;
            int sum = 0;

            for (int i = 0; i <= threads; i++)
            {
                var part = _array.Skip(i * partSize).Take(partSize).ToArray();
                var thread  = new Thread(() => ArraySum(part, ref sum));
                thread.Start();
                threadPool.Add(thread);
            }

            threadPool.ForEach(thread => thread.Join());

            return sum;
        }

        private static int PLinqCalculation(int[] _array) => _array.AsParallel().Sum();
    }
}
