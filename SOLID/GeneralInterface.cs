using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    abstract class GeneralInterface : IInterface
    {
        protected int Read(Func<int> read)
        {
            try
            {
                return read.Invoke();
            }
            catch(Exception ex)
            {
                Write(ex.ToString());
                return 0;
            }
        }

        protected void Write(Action write)
        {
            try
            {
                write.Invoke();
            }
            catch (Exception ex)
            {
                Write(ex.ToString());
            }
        }

        abstract public void Write(string str);
        abstract public int Read();
    }
}
