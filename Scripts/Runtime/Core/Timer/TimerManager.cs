using System.Collections.Generic;

namespace Cofdream.BaseFramework.Timer
{
    // 对List 的优化停止
    public static class TimerManager
    {
        private static List<ITimer> timers;
        //private static int timerCount;

        private static int minCapacity = 20;
        //private static float disposeCurrentTime;    // 释放时间计时 单位：s
        //private static float disposeTotalTime = 60; // 释放时间     单位：s
        public static bool TimerState { get; set; }
        static TimerManager()
        {
            timers = new List<ITimer>(minCapacity);

            TimerState = true;
            FrameUpdater.UpdataAction += UpdateTime;
        }
        private static void UpdateTime()
        {
            if (!TimerState) return;

            var delta = FrameUpdater.DeltaTime;

            for (int i = 0; i < timers.Count; i++)
            {
                var timer = timers[i];
                bool isRemove = timer.Update(delta);
                if (isRemove)
                {
                    timer.Dispose();
                    timers[i] = null;
                    timers.RemoveAt(i);
                }
                else
                {
                }
            }

            // 可能不对，暂时不释放

            // 在指定释放时间内，检测池大小是否过大。释放一半数组
            //int capacity = timers.Capacity;
            //if (timerCount < capacity * 0.25f)
            //{
            //    disposeCurrentTime += delta;
            //    if (disposeCurrentTime > disposeTotalTime)
            //    {
            //        disposeCurrentTime = 0;
            //        int curCapacity = (int)(timerCount * 0.5f);
            //        timers.Capacity = curCapacity > minCapacity ? curCapacity : minCapacity;
            //    }
            //}
            //else
            //{
            //    disposeCurrentTime = 0f;
            //}
        }


        public static void StartTimer(this ITimer timer)
        {
            //if (timerCount < timers.Count)
            //{
            //    timers[timerCount] = timer;
            //}
            //else
            //{
            //    timers.Add(timer);
            //}
            //timerCount++;

            timers.Add(timer);
        }
        public static void StartTimer(this IEnumerable<ITimer> timers)
        {

            TimerManager.timers.AddRange(timers);

            //TimerManager.timers.RemoveRange();

            //int surplusCount = TimerManager.timers.Count - timerCount;
            //if (surplusCount > 0)
            //{

            //}

            //TimerManager.timers.AddRange(timers);
            //// 增加 相比之前增加的数量
            //int addCount = TimerManager.timers.Count - count;

            //timerCount = timerCount + ;
        }
    }
}
