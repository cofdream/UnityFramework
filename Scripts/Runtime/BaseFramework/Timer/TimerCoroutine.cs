using Cofdream.BaseFramework.ObjectPool;
using System;

namespace Cofdream.BaseFramework.Timer
{
    public class TimerCoroutine : ITimer
    {
        /// <summary>
        /// 每次更新等待的时间
        /// </summary>
        public float WaitingTime;
        /// <summary>
        /// 已经过的时间
        /// </summary>
        public float ElapsedTime;
        /// <summary>
        /// 计时回调
        /// </summary>
        public Func<TimerCoroutine, bool> Callback;
        /// <summary>
        /// 暂停
        /// </summary>
        public bool IsPause;

        private static IObjectPool<TimerCoroutine> pool;
        static TimerCoroutine()
        {
            ObjectPool<TimerCoroutine> objectPool = new ObjectPool<TimerCoroutine>();
            objectPool.Initialize(Create, null, null, null, 20);

            pool = objectPool;
        }
        static TimerCoroutine Create()
        {
            return new TimerCoroutine();
        }
        public static TimerCoroutine GetTimer()
        {
            var timer = pool.Allocate();
            timer.WaitingTime = 0;
            timer.ElapsedTime = 0;
            timer.Callback = null;
            timer.IsPause = true;

            return pool.Allocate();
        }


        public void Run(float waitingTime, Func<TimerCoroutine, bool> callback, float useTime = 0f)
        {
            WaitingTime = waitingTime;
            ElapsedTime = useTime;
            Callback = callback;

            IsPause = false;

            TimerManager.StartTimer(this);
        }
        public bool Update(float time)
        {
            if (IsPause) return false;

            ElapsedTime += time;
            if (WaitingTime <= ElapsedTime)
            {
                ElapsedTime = 0;

                return Callback.Invoke(this);
            }
            return false;
        }

        public void Dispose()
        {
            Callback = null;

            pool.Release(this);
        }
    }
}