using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VowelAServer.Server.Utils
{
    public class ReflectionHelper
    {
        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            var objects  = new List<T>();
            var assembly = Assembly.GetAssembly(typeof(T));
            if (assembly == null) return objects;
            foreach (var type in assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects;
        }
    }
}