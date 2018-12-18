using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITests.TestClasses
{
    public interface ISomething3<T> where T : SomethingAbstract1
    {
        void DoIt(T t);
    }
}
