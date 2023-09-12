using System;
using System.Collections.Generic;

namespace Cofdream.BaseFramework.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T>
    {
        private Stack<T> objectStack;

        public event Func<T> CreateObjectAction;
        public event Action<T> DestoryObjectAction;

        public event Action<T> GetObjectAction;
        public event Action<T> ReleaseObjectAction;

        /// <summary>
        /// 池最大容量,超出的对象再回收以后立即删除
        /// </summary>
        private int poolMaxCount;

        public void Initialize(Func<T> createObjectAction, Action<T> destoryObjectAction, Action<T> getObjectAction, Action<T> releaseObjectAction,
                          int capacity = 10, int poolMaxCount = int.MaxValue)
        {
            CreateObjectAction = createObjectAction;
            DestoryObjectAction = destoryObjectAction;
            GetObjectAction = getObjectAction;
            ReleaseObjectAction = releaseObjectAction;

            objectStack = new Stack<T>(capacity);
            this.poolMaxCount = poolMaxCount;
        }
        public T Allocate()
        {
            T _object;
            if (objectStack.Count != 0)
            {
                _object = objectStack.Pop();
            }
            else
            {
                if (CreateObjectAction == null)
                {
                    _object = default;
                }
                else
                {
                    _object = this.CreateObjectAction.Invoke();
                }
            }

            GetObjectAction?.Invoke(_object);
            return _object;
        }
        public void Release(T _object)
        {
            ReleaseObjectAction?.Invoke(_object);

            if (objectStack.Count >= poolMaxCount)
            {
                DestoryObjectAction?.Invoke(_object);
            }
            else
            {
                objectStack.Push(_object);
            }
        }
        public void ClearPool()
        {
            if (DestoryObjectAction != null)
            {
                foreach (var _object in objectStack)
                {
                    DestoryObjectAction.Invoke(_object);
                }
            }

            objectStack.Clear();
        }
    }
}