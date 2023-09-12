using Cofdream.BaseFramework.ObjectPool;
using System;

namespace Cofdream.BaseFramework.Timer
{
    public class Timer : ITimer
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
        /// 剩余需要计时间的次数
        /// </summary>
        public ushort Number;
        /// <summary>
        /// 计时回调
        /// </summary>
        public Action Callback;
        /// <summary>
        /// 暂停
        /// </summary>
        public bool IsPause;

        private static IObjectPool<Timer> pool;
        static Timer()
        {
            ObjectPool<Timer> objectPool = new ObjectPool<Timer>();
            objectPool.Initialize(Create, null, null, null, 20);

            pool = objectPool;
        }
        static Timer Create()
        {
            return new Timer();
        }
        public static Timer GetTimer()
        {
            var timer = pool.Allocate();
            timer.WaitingTime = 0;
            timer.ElapsedTime = 0;
            timer.Number = 1;
            timer.Callback = null;
            timer.IsPause = true;

            return pool.Allocate();
        }

        public void Run(float waitingTime, Action callback, ushort number = 1, float useTime = 0f)
        {
            WaitingTime = waitingTime;
            ElapsedTime = useTime;
            Number = number;
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
                if (Callback == null) return true;

                Callback.Invoke();
                ElapsedTime = WaitingTime - ElapsedTime;
                Number--;
                if (Number == 0)
                {
                    return true;
                }
                return false;
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