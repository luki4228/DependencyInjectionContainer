using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
            foreach (Type depType in _config.DContainer.Keys)
            {
                if (depType.IsValueType)
                {
                    throw new ArgumentException("TDependency must be a ref type");
                }

                foreach (Type implemType in _config.DContainer[depType])
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

            List<TDependency> res = new List<TDependency>();

            if (depType.IsGenericType && config.DContainer.ContainsKey(depType.GetGenericTypeDefinition()))
            {

                var implements = config.DContainer[depType.GetGenericTypeDefinition()];
                foreach (var implement in implements)
                {
                    var genericType = implement.MakeGenericType(depType.GetGenericArguments());
                    var resolved = (TDependency)CreateInstance(genericType);
                    res.Add(resolved);
                }

                return res;

            }
            else
            if (!config.DContainer.ContainsKey(depType))
            {
                throw new KeyNotFoundException($"Dependency {depType.ToString()} is not registered.");
            }

            foreach (var implement in config.DContainer[depType])
            {

                TDependency resolved;
                if (config.ObjectSettings[implement])
                {
                    if (config.OContainer[implement].instance == null)
                    {
                        lock (config.OContainer[implement].syncRoot)
                        {
                            if (config.OContainer[implement].instance == null)
                            {
                                config.OContainer[implement].instance = CreateInstance(implement);
                            }
                        }
                    }

                    resolved = (TDependency)config.OContainer[implement].instance;

                }
                else
                {
                    resolved = (TDependency)CreateInstance(implement);
                }

                res.Add(resolved);
            }

            return res;

        }

        public object ResolveT(Type t)
        {
            var rMethod = typeof(DIProvider).GetMethod("Resolve");
            var rType = rMethod.MakeGenericMethod(t);

            var lType = typeof(List<>);
            var constructedLType = lType.MakeGenericType(t);

            dynamic resolved = Convert.ChangeType(rType.Invoke(this, null), constructedLType);

            if (resolved.Count > 1)
            {
                return resolved;
            }
            else
            {
                return resolved[0];
            }
        }

        private ConstructorInfo GetBestConstructor(ConstructorInfo[] constructors)
        {
            ConstructorInfo bestConstructor = null;

            var sortedConstructors = constructors.OrderBy(x => x.GetParameters().Length);
            bestConstructor = constructors.Last();
            return bestConstructor;
        }

        private object CreateInstance(Type t)
        {
            ConstructorInfo[] constructors = t.GetConstructors();
            ConstructorInfo constructor = GetBestConstructor(constructors);

            if (constructor == null)
            {
                throw new InvalidOperationException(string.Format("Can not create instance of type {0}. Satisfying constructor does not exist.", t));
            }
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parameterInstances = new object[parameters.Length];

            int i = 0;
            foreach (var param in parameters)
            {
                Type paramType = param.ParameterType;

                if (paramType.IsGenericType && paramType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                {
                    paramType = paramType.GenericTypeArguments[0];
                }

                if (paramType.IsGenericType)
                {
                    parameterInstances[i] = ResolveT(paramType.GetGenericTypeDefinition()
                        .MakeGenericType(paramType.GenericTypeArguments)); ;
                }
                else
                {
                    if (config.DContainer.ContainsKey(paramType))
                    {
                        parameterInstances[i] = ResolveT(paramType);
                    }
                }
                i++;
            }

            var instance = constructor.Invoke(parameterInstances);
            return instance;
        }
    }
}
