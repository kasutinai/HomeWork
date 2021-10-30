using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    class ConsoleInterface : GeneralInterface
    {
        public override void Write(string str) => Write(() => Console.WriteLine(str));
        public override int Read() => Read(() => int.Parse(Console.ReadLine()));
    }
}
