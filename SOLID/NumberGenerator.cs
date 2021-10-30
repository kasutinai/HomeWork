using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    class NumberGenerator : INumberGenerator
    {
        virtual public int Generate()
        {
            return new Random().Next(AppSettings.GetMin(), AppSettings.GetMax() + 1);
        }
    }
}
