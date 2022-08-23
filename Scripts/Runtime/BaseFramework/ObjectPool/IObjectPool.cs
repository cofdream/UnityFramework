using System;

namespace Cofdream.BaseFramework.ObjectPool
{
    public interface IObjectPool<T>
    {
        T Allocate();
        void Release(T _object);
        void ClearPool();
    }
}