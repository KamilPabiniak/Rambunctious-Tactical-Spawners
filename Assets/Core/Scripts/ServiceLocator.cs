using System;
using System.Collections.Generic;

namespace Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static bool HasService<T>()
        {
            return Services.ContainsKey(typeof(T));
        }
        
        public static void RegisterService<T>(T service)
        {
            Type type = typeof(T);
            Services.TryAdd(type, service);
        }

        public static T GetService<T>()
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            throw new Exception("Service of type " + type + " not registered.");
        }
    }
}