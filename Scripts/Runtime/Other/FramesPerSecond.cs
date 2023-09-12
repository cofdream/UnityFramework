using UnityEngine;

namespace Cofdream.BaseFramework.Other
{
    /// <summary>
    /// FPS
    /// </summary>
    public class FramesPerSecond
    {
        private float _unscaleTime;
        private int _frameCount;

        private float _time;

        public float FPS { get; private set; }
        public float MS { get; private set; }

        public float FPS2 { get; private set; }
        public float MS2 { get; private set; }

        private void UpdateFPS()
        {
            _frameCount++;
            _unscaleTime += Time.unscaledDeltaTime;
            if (_unscaleTime >= 1)
            {
                FPS = _frameCount / _unscaleTime;
                MS = _unscaleTime * 1000f / _frameCount;
                _frameCount = 0;
                _unscaleTime -= 1f;
            }
        }

        private void UpdateFPS2()
        {
            _time += (Time.unscaledDeltaTime - _time) * 0.1f;

            FPS2 = 1f / _time;
            MS2 = _time * 1000f;
        }
    }
}