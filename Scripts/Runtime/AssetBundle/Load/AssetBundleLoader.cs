using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cofdream.Asset
{
    public class AssetBundleLoader : IAssetLoader
    {
        //private delegate void LoadedCallBack();
        private delegate Object LoadedCallBack();

        private uint referenceCount;
        private LoadState loadState;
        private string assetBundleName;

        private AssetBundle assetBundle;
        private AssetBundleLoader[] assetBundleLoadDependencies;

        private AssetBundleCreateRequest assetBundleCreateRequest;
        private ushort dependLoadedCount;
        private UnityAction dependLoadedCallBack;

        private LoadedCallBack loadedCallBack;


        private Dictionary<string, Object> loadedObjects;

        private static Dictionary<string, AssetBundleLoader> allAssetBundleLoaders = new Dictionary<string, AssetBundleLoader>();

        private static string folderName = AssetUtil.GetAssetSaveFolderName(Application.platform);
        private static string rootPath =
#if UNITY_EDITOR
           System.IO.Directory.GetParent(Application.dataPath) + "/BuildAssetBundle/" + folderName + "/";
#else
           Application.persistentDataPath + "/BuildAssetBundle/" + folderName + "/";
#endif

        public uint ReferenceCount => referenceCount;
        public LoadState LoadState => loadState;
        public string AssetBundleName => assetBundleName;
        public AssetBundle AssetBundle => assetBundle;
        public AssetBundleLoader[] AssetBundleLoadDependencies => assetBundleLoadDependencies;
        public Dictionary<string, Object> LoadedObjects => loadedObjects;


        AssetBundleLoader() { }

        public static AssetBundleLoader GetLoader(string assetBundleName)
        {
            if (allAssetBundleLoaders.TryGetValue(assetBundleName, out AssetBundleLoader assetBundleLoad) == false)
            {
                //todo 验证是ab包名正确性 

                assetBundleLoad = new AssetBundleLoader();
                allAssetBundleLoaders.Add(assetBundleName, assetBundleLoad);

                assetBundleLoad.referenceCount = 0;
                assetBundleLoad.loadState = LoadState.NotLoad;
                assetBundleLoad.assetBundleName = assetBundleName;
                assetBundleLoad.loadedObjects = new Dictionary<string, Object>();

                assetBundleLoad.referenceCount++;
                return assetBundleLoad;
            }
            else
            {
                assetBundleLoad.referenceCount++;
                return assetBundleLoad;
            }
        }

        public static void Put(AssetBundleLoader assetBundleLoad)
        {
            if (assetBundleLoad.referenceCount == 1)
            {
                assetBundleLoad.referenceCount = 0;

                foreach (var assetBundleLoadDeoend in assetBundleLoad.assetBundleLoadDependencies)
                {
                    Put(assetBundleLoadDeoend);
                }

                assetBundleLoad.loadState = LoadState.Unload;
                assetBundleLoad.assetBundle.Unload(true);
                assetBundleLoad.assetBundle = null;
                assetBundleLoad.loadedObjects.Clear();
            }
            else
            {
                assetBundleLoad.referenceCount--;
            }
        }


        private void Load()
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(rootPath + folderName);
                    var assetBundleManifest = assetBundleMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    if (dependencies.Length > 0)
                    {
                        assetBundleLoadDependencies = new AssetBundleLoader[dependencies.Length];
                    }
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        var assetBundleLoad = GetLoader(dependencies[i]);
                        assetBundleLoad.Load();
                        assetBundleLoadDependencies[i] = assetBundleLoad;
                    }

                    assetBundleMain.Unload(true);

                    assetBundle = AssetBundle.LoadFromFile(rootPath + assetBundleName);

                    if (assetBundle == null)
                    {
                        loadState = LoadState.LoadError;
                    }
                    else
                    {
                        loadState = LoadState.Loaded;
                    }

                    break;
                case LoadState.Loading:
                    // todo 同步加载的ab/依赖ab 再加载中
                    Debug.LogError("同步加载的ab/依赖ab 再加载中");
                    break;
                case LoadState.Loaded:
                    break;
                case LoadState.LoadError:
                    break;
            }
        }
        public Object Load(string assetName, System.Type type)
        {
            Load();

            if (loadedObjects.TryGetValue(assetName, out Object asset) == false)
            {
                asset = assetBundle.LoadAsset(assetName, type);
                loadedObjects.Add(assetName, asset);
            }
            return asset;
        }

        public static void Test()
        {
            //模拟外部加载
            var assetBundleLoader = GetLoader("AB包名字");

            assetBundleLoader.LoadAsync("资源名字", null);
        }

        public void LoadAsync(LoadedCallBack loadedCallBack)
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    loadState = LoadState.Loading;

                    assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(rootPath + assetBundleName);
                    assetBundleCreateRequest.completed += Loaded;

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(rootPath + folderName);
                    var assetBundleManifest = assetBundleMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    dependLoadedCount = (ushort)dependencies.Length;
                    for (int i = 0; i < dependLoadedCount; i++)
                    {
                        var assetBundleLoad = GetLoader(dependencies[i]);
                        assetBundleLoad.LoadAsync(DependLoaded);
                        assetBundleLoadDependencies[i] = assetBundleLoad;
                    }

                    assetBundleMain.Unload(true);

                    break;
                case LoadState.Loading:
                    dependLoadedCallBack += dependLoaded;
                    break;
                case LoadState.Loaded:
                    dependLoaded?.Invoke();
                    break;
                case LoadState.LoadError:
                    break;
            }
        }

        public void o

        private void Loaded(AsyncOperation asyncOperation)
        {
            if (asyncOperation.isDone)
            {
                //依赖先加载完成
                if (dependLoadedCount == 0)
                {
                    Loaded();
                }
            }
        }

        private Object DependLoaded(System.Type type)
        {
            dependLoadedCount--;
            if (dependLoadedCount == 0)
            {
                if (assetBundleCreateRequest.isDone)
                {
                    //本体先加载完成
                    Loaded();
                }
            }
        }

        private void Loaded()
        {
            if (LoadState == LoadState.Loading)
            {
                dependLoadedCallBack?.Invoke();
                dependLoadedCallBack = null;

            }
            else
                Debug.LogError($"加载状态异常，请检查整个加载流程。loadState {loadState}");
        }

        public void UnloadAllLoadedObjects()
        {
            Put(this);
        }

        public void LoadAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}