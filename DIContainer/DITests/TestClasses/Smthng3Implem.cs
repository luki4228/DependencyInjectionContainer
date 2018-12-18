using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITests.TestClasses
{
    class Smthng3Implem<T> : ISomething3<T> where T : SomethingAbstract1
    {
        public T smthng;
        
        public Smthng3Implem(T _smthng)
        {
            smthng = _smthng;
        }

        public void DoIt(T t)
        {
            smthng.DoSmthng();
        }
    }
}
