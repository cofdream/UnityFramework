using System;
using System.Reflection;

namespace Cofdream.BaseFramework.Singleton
{
    public abstract class SingletonNoReflection<T> where T : SingletonNoReflection<T>, new()
    {
        private static readonly T instance;
        public static T Instance { get { return instance; } }

        static SingletonNoReflection()
        {
            instance = new T();
            instance.InitSingleton();
        }

        protected virtual void InitSingleton() { }
    }
}