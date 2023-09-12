using UnityEngine;

namespace Cofdream.BaseFramework.UnityEngine.Singleton
{
    [System.Obsolete("", true)]
    public abstract class SingletonMonoBehaviour<T> //: MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        //        private static T instance;

        //        public static T Instance { get { return instance; } }


        //        private void Awake()
        //        {
        //            if (instance == null)
        //            {
        //                var instances = Object.FindObjectsOfType<T>();

        //                if (instances.Length != 0)
        //                {
        //                    for (int i = 0; i < instances.Length; i++)
        //                    {
        //                        Destroy(instances[i]);
        //                    }

        //                    // 单例对象可能没被释放掉，需要重启引擎
        //                    Debug.LogError("There are multiple singleton objects. The singleton objects may not be released. It needs to restart the unity engine to release them!");
        //                }

        //                var gameObject = new GameObject($"[{typeof(T)} SingletonMonoBehaviour (No Copy!)]");
        //                instance = gameObject.AddComponent<T>();
        //                Object.DontDestroyOnLoad(gameObject);
        //            }
        //            else
        //            {
        //                if (System.Object.Equals(instance, null) == false)
        //                {
        //                    // C#对象还在，但是 引擎对象已经不存在了
        //                    Debug.LogError("C# object existed,But UnityEngine object no exists!");
        //                }
        //            }
        //        }

        //#if UNITY_EDITOR
        //        protected virtual void Reset()
        //        {
        //            DestroyImmediate(this);
        //            Debug.LogWarning($"You cannot manually add a singleton mono Component!!\n<color=red>Type:<{typeof(T)}></color>");
        //        }
        //#endif
    }
}