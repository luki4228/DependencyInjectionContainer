using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DIProvider
    {
        DIConfig config;

        public DIProvider(DIConfig _config)
        {
            config = _config;
        }

        public TImplementation Resolve<TImplementation>()
        {
            throw new NotImplementedException();
        }
    
    }
}
