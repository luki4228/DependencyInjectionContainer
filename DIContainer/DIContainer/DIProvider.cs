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
            validateConfig(_config);
            config = _config;
        }

        public void validateConfig(DIConfig _config)
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

        public List<TDependency> Resolve<TDependency>()
        {
            Type depType = typeof(TDependency);
            if (depType.IsGenericType && depType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
            {
                depType = depType.GenericTypeArguments[0];
            }

            if (depType.IsGenericType)
            {
                foreach (var type in depType.GenericTypeArguments)
                {
                    if (!config.DContainer.ContainsKey(type))
                    {
                        throw new KeyNotFoundException($"Dependency {type.ToString()} is not registered.");
                    }
                }
            }
            else
            if (!config.DContainer.ContainsKey(depType))
            {
                throw new KeyNotFoundException($"Dependency {depType.ToString()} is not registered.");
            }

            List<TDependency> result = new List<TDependency>();

            

            return result;

        }

        public object ResolveT(Type t)
        {
            var rMethod = typeof(DIProvider).GetMethod("Resolve");
            var rType = rMethod.MakeGenericMethod(t);

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);

            dynamic resolved = Convert.ChangeType(rType.Invoke(this, null), constructedListType);

            if (resolved.Count > 1)
            {
                return resolved;
            }
            else
            {
                return resolved[0];
            }
        }
    }
}
