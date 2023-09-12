using System;

namespace Cofdream.BaseFramework
{
    public class FrameUpdater
    {
        protected static float deltaTime;
        public static float DeltaTime { get => deltaTime; private set => deltaTime = value; }

        public static event Action UpdataAction;

        protected FrameUpdater() { }
    }
}