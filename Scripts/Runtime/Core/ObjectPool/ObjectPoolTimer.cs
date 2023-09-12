using System;
using System.Collections.Generic;

namespace Cofdream.BaseFramework.ObjectPool
{
    // TODO 释放策略修改

    public class ObjectPoolTimer<T> : IObjectPool<T>
    {
        private Stack<T> objectStack;

        public Func<T> CreateObjectAction;
        public Action<T> DestoryObjectAction;

        public Action<T> GetObjectAction;
        public Action<T> ReleaseObjectAction;

        private int poolMaxCount;

        public float WaitingTime { get; set; }
        private float elapsedTime;
        private ushort objectReferenceCount;
        public int PoolMinCount { get; set; }

        private bool autoReleaseObjectPoolState = false;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="createObjectAction">创建对象</param>
        /// <param name="destoryObjectAction">删除对象</param>
        /// <param name="getObjectAction">取出对象</param>
        /// <param name="releaseObjectAction">回收对象</param>
        /// <param name="capacity">池初始大小（优化池）</param>
        /// <param name="poolMaxCount">池最大容留，超过容量的对象再回收后立即删除。</param>
        /// <param name="poolMinCount">池最小容留（优化池）</param>
        /// <param name="waitingTime">池自动释放的等待时间（优化池）</param>
        public void Initialize(Func<T> createObjectAction, Action<T> destoryObjectAction = null, Action<T> getObjectAction = null, Action<T> releaseObjectAction = null
                                , int capacity = 10, int poolMaxCount = 10,
                                int poolMinCount = 10, float waitingTime = 60)
        {
            CreateObjectAction = createObjectAction;
            DestoryObjectAction = destoryObjectAction;
            GetObjectAction = getObjectAction;
            ReleaseObjectAction = releaseObjectAction;

            objectStack = new Stack<T>(capacity);
            this.poolMaxCount = poolMaxCount;

            PoolMinCount = poolMinCount;
            WaitingTime = waitingTime;
        }

        public T Allocate()
        {
            T _object;
            if (objectStack.Count != 0)
                _object = objectStack.Pop();
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

            objectReferenceCount++;
            if (objectReferenceCount == 1 && autoReleaseObjectPoolState == true)
            {
                FrameUpdater.UpdataAction -= UpdateObjectPool;
                autoReleaseObjectPoolState = false;
            }

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

            objectReferenceCount--;
            if (objectReferenceCount == 0 && objectStack.Count > PoolMinCount && autoReleaseObjectPoolState == false)
            {
                autoReleaseObjectPoolState = true;
                elapsedTime = 0;
                FrameUpdater.UpdataAction += UpdateObjectPool;
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
        private void UpdateObjectPool()
        {
            float time = FrameUpdater.DeltaTime;
            elapsedTime += time;
            if (WaitingTime <= elapsedTime)
            {
                ReleaseObjectPool();
            }
        }
        private void ReleaseObjectPool()
        {
            Stack<T> newObjectStatck = new Stack<T>(PoolMinCount);
            for (int i = 0; i < PoolMinCount; i++)
            {
                newObjectStatck.Push(objectStack.Pop());
            }

            ClearPool();

            if (autoReleaseObjectPoolState == true)
            {
                FrameUpdater.UpdataAction -= UpdateObjectPool;
                autoReleaseObjectPoolState = false;
            }

            objectStack = newObjectStatck;
        }
    }
}