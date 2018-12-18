using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DITests.TestClasses
{
    public class Something2 : SomethingAbstract1
    {
        public ISomeKek smthng1;
        public List<ISomeLol> smthng2;

        public override void DoSmthng()
        {
            
        }

        public Something2(ISomeKek _smthng1, IEnumerable<ISomeLol> _smthng2)
        {
            smthng1 = _smthng1;
            smthng2 = (List<ISomeLol>) _smthng2;
        }
    }
}
