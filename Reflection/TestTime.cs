using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
	static class TestTime
	{
		public static void Test(F f, int iterations = 0)
		{
			var cSVSerializer = new Serializer<F>();
			string s = "";

			try
			{
				Stopwatch stopwatch = new Stopwatch();

				stopwatch.Start();
				for (int i = 0; i < iterations; i++)
				{
					s = cSVSerializer.Serialize(f);
				}
				stopwatch.Stop();
				Console.WriteLine($"My serialization time: {stopwatch.ElapsedMilliseconds}");

				stopwatch.Restart();
				for (int i = 0; i < iterations; i++)
				{
                    var desrialized = cSVSerializer.Deserialize(s);
				}
				stopwatch.Stop();
				Console.WriteLine($"My deserialization time: {stopwatch.ElapsedMilliseconds}");

				stopwatch.Restart();
				for (int i = 0; i < iterations; i++)
				{
					s = JsonConvert.SerializeObject(f);
				}
				stopwatch.Stop();
				Console.WriteLine($"JSON serialization time: {stopwatch.ElapsedMilliseconds}");

				stopwatch.Restart();
				for (int i = 0; i < iterations; i++)
				{
					var desrializedByJson = JsonConvert.DeserializeObject(s);
				}
				stopwatch.Stop();
				Console.WriteLine($"JSON deserialization time: {stopwatch.ElapsedMilliseconds}");				
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}
	}	
}
