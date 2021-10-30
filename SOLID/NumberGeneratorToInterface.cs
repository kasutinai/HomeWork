using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    class NumberGeneratorToInterface : NumberGenerator
    {
        private int _result;
        private IInterface _interface;

        override public int Generate()
        {
            _result = base.Generate();
            WriteToInterface();
            return _result;
        }
        public void WriteToInterface()
        {
            _interface?.Write($"Generated number: {_result}");
        }

        public void setInterface(IInterface curInterface) => _interface = curInterface;
    }
}
