using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DIProvider
    {
        private DIConfig config;

        public DIProvider(DIConfig _config)
        {
            config = _config;
        }

        public void validateConfiguration(DIConfig _config)
        {
            foreach (Type depType in config.DContainer.Keys)
            {
                if (depType.IsValueType)
                {
                    throw new ArgumentException("TDependency must be a ref type");
                }

                foreach (Type implemType in config.DContainer[depType])
                {
                    if (!depType.IsAssignableFrom(implemType) && !depType.IsGenericTypeDefinition)
                    {
                        throw new ArgumentException("TImplementation must implement or be inherited from TDependency.");
                    }

                    if (implemType.IsAbstract || !implemType.IsClass)
                    {
                        throw new ArgumentException("TImplementation must be class(not abstract)");
                    }

                }
            }
        }

        public TImplementation Resolve<TImplementation>()
        {
            throw new NotImplementedException();
        }
    
    }
}
