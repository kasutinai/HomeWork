using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
			var f = new F().Get();

			TestTime.Test(f, 10000);

			Console.ReadLine();
		}
	}
}
