﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class DIConfig
    {
        public Dictionary<Type, List<Type>> DContainer;
        public Dictionary<Type, bool> ObjectSettings;
        public Dictionary<Type, SingletonContainer> OContainer;

        public void Register<TDependency, TImplementation>(bool objectSettings)
        {
            Type tDependency = typeof(TDependency);
            Type tImplementation = typeof(TImplementation);
            if (!DContainer.ContainsKey(tDependency))
            {
                DContainer[tDependency] = new List<Type>();
                DContainer[tDependency].Add(tImplementation);
            }
            else
            {
                if (!DContainer[tDependency].Contains(tImplementation))
                {
                    DContainer[tDependency].Add(tImplementation);
                }
            }
            ObjectSettings[tImplementation] = objectSettings;

            if (objectSettings)
            {
                OContainer[tImplementation] = new SingletonContainer();
            }
        }
        public void Register(Type tDependency, Type tImplementation)
        {
            if (!DContainer.ContainsKey(tDependency))
            {
                DContainer[tDependency] = new List<Type>();
            }
            if (!DContainer[tDependency].Contains(tImplementation))
            {
                DContainer[tDependency].Add(tImplementation);
            }
        }

        public DIConfig()
        {
            DContainer = new Dictionary<Type, List<Type>>();
            OContainer = new Dictionary<Type, SingletonContainer>();
            ObjectSettings = new Dictionary<Type, bool>();
        }
    }
}
