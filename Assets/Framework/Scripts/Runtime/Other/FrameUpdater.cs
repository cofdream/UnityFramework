using UnityEngine;

namespace Cofdream.BaseFramework.Other
{
    /// <summary>
    /// 使用unity函数来更新
    /// </summary>
    public class FrameUpdater : MonoBehaviour
    {
        //static FrameUpdater()
        //{
        //    Object.DontDestroyOnLoad(new GameObject("FrameUpdater (instance)").AddComponent<FrameUpdaterComponent>());
        //}

        //[DisallowMultipleComponent]
        //private sealed class FrameUpdaterComponent : MonoBehaviour
        //{
        //    private void Update()
        //    {
        //        deltaTime = Time.deltaTime;

        //        UpdataAction?.Invoke();
        //    }
        //}

        //protected static float deltaTime;
        //public static float DeltaTime { get => deltaTime; private set => deltaTime = value; }

        //public static event Action UpdataAction;

        //protected FrameUpdater() { }
    }


}