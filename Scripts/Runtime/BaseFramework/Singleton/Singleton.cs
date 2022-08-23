using System;
using System.Reflection;

namespace Cofdream.BaseFramework.Singleton
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static readonly T instance;
        public static T Instance { get { return instance; } }

        static Singleton()
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception($"{typeof(T)} need constructor");
            }

            instance = ctor.Invoke(null) as T;

            instance.InitSingleton();
        }

        protected virtual void InitSingleton() { }

        // 提供一个空函数来预加载实例
        public static void PreloadSingleton() { }
    }
}