using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DIProvider
    {
        public DIProvider(DIConfig configuration)
        {

        }

        public TImplementation Resolve<TImplementation>()
        {
            throw new NotImplementedException();
        }
    
    }
}
