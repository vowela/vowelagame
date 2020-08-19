using System;

namespace VowelAServer.Utilities.Interfaces
{
    public abstract class SingletonController<T> where T : SingletonController<T>, new()
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => new T());
        
        public static T Instance => lazy.Value;
    }
}