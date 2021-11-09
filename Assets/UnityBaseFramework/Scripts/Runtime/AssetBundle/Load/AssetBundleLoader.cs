using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cofdream.Asset
{
    public class AssetBundleLoader : IAssetLoader
    {
        private uint referenceCount;
        private LoadState loadState;
        private string assetBundleName;

        private AssetBundle assetBundle;
        private AssetBundleLoader[] assetBundleLoadDependencies;

        private AssetBundleCreateRequest assetBundleCreateRequest;
        private ushort dependLoadedCount;
        private UnityAction<IAssetLoader> cacheLoaded;
        private UnityAction cacheDependLoaded;

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


        public Object Load(string assetName, System.Type type)
        {
            LoadAssetBundle();
            if (assetBundle == null)
            {
                return null;
            }
            return LoadAsset(assetName, type);
        }
        private void LoadAssetBundle()
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    loadState = LoadState.Loading;

                    assetBundle = AssetBundle.LoadFromFile(rootPath + assetBundleName);

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(rootPath + folderName);
                    var assetBundleManifest = assetBundleMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    assetBundleMain.Unload(true);

                    // 加载依赖
                    dependLoadedCount = (ushort)dependencies.Length;
                    if (dependLoadedCount > 0)
                    {
                        assetBundleLoadDependencies = new AssetBundleLoader[dependLoadedCount];

                        for (int i = 0; i < dependLoadedCount; i++)
                        {
                            var assetBundleLoad = GetLoader(dependencies[i]);
                            assetBundleLoad.LoadAssetBundle();
                            assetBundleLoadDependencies[i] = assetBundleLoad;
                        }

                        dependLoadedCount = 0;
                    }

                    loadState = LoadState.Loaded;

                    break;
                case LoadState.Loading:
                    Debug.LogError("资源已经在异步加载中，无法同步加载出数据。请检查依赖文件或加载函数。");
                    break;
                case LoadState.Loaded:
                    break;
            }
        }


        public void LoadAsync(UnityAction<IAssetLoader> loaded)
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    loadState = LoadState.Loading;

                    assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(rootPath + assetBundleName);
                    assetBundleCreateRequest.completed += AssetBundleLoadedCallback;

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(rootPath + folderName);
                    var assetBundleManifest = assetBundleMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    assetBundleMain.Unload(true);

                    //异步加载依赖
                    dependLoadedCount = (ushort)dependencies.Length;
                    if (dependLoadedCount > 0)
                    {
                        assetBundleLoadDependencies = new AssetBundleLoader[dependLoadedCount];
                        for (int i = 0; i < dependLoadedCount; i++)
                        {
                            var assetBundleLoad = GetLoader(dependencies[i]);
                            assetBundleLoad.DependLoadAsync(DependLoadedCallback);
                            assetBundleLoadDependencies[i] = assetBundleLoad;
                        }
                    }

                    break;
                case LoadState.Loading:

                    if (loaded != null)
                    {
                        // 保存需要回调的事件
                        cacheLoaded += loaded;
                    }

                    break;
                case LoadState.Loaded:

                    //处理缓存的回调事件
                    if (cacheLoaded != null)
                    {
                        cacheLoaded.Invoke(this);
                        cacheLoaded = null;
                    }

                    break;
            }
        }
        
        private void DependLoadAsync(UnityAction dependLoaded)
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    loadState = LoadState.Loading;

                    assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(rootPath + assetBundleName);
                    assetBundleCreateRequest.completed += DependLoadedCallback;

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(rootPath + folderName);
                    var assetBundleManifest = assetBundleMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    assetBundleMain.Unload(true);

                    //异步加载依赖
                    dependLoadedCount = (ushort)dependencies.Length;
                    if (dependLoadedCount > 0)
                    {
                        assetBundleLoadDependencies = new AssetBundleLoader[dependLoadedCount];
                        for (int i = 0; i < dependLoadedCount; i++)
                        {
                            var assetBundleLoad = GetLoader(dependencies[i]);
                            assetBundleLoad.DependLoadAsync(DependLoadedCallback);
                            assetBundleLoadDependencies[i] = assetBundleLoad;
                        }
                    }

                    break;
                case LoadState.Loading:

                    // 保存依赖加载完的回调的事件
                    cacheDependLoaded += dependLoaded;

                    break;
                case LoadState.Loaded:

                    dependLoaded?.Invoke();

                    break;
            }
        }
        private void AssetBundleLoadedCallback(AsyncOperation asyncOperation)
        {
            if (asyncOperation.isDone)
            {
                //依赖已加载完成
                if (dependLoadedCount == 0)
                {
                    if (loadState == LoadState.Loading)
                    {
                        assetBundle = assetBundleCreateRequest.assetBundle;
                        loadState = LoadState.Loaded;
                    }
                    else
                    {
                        Debug.LogError($"加载状态异常，请检查整个加载流程。loadState {loadState}");
                    }
                }
            }
        }
        private void DependLoadedCallback(AsyncOperation asyncOperation)
        {
            if (asyncOperation.isDone)
            {
                //依赖已加载完成
                if (dependLoadedCount == 0)
                {
                    //把缓存的回调都清除
                    if (cacheDependLoaded != null)
                    {
                        cacheDependLoaded?.Invoke();
                        cacheDependLoaded = null;
                    }

                    if (loadState == LoadState.Loading)
                    {
                        assetBundle = assetBundleCreateRequest.assetBundle;
                        loadState = LoadState.Loaded;
                    }
                    else
                    {
                        Debug.LogError($"加载状态异常，请检查整个加载流程。loadState {loadState}");
                    }
                }
            }
        }
        private void DependLoadedCallback()
        {
            dependLoadedCount--;
            if (dependLoadedCount == 0)
            {
                //本体已加载完成
                if (assetBundleCreateRequest.isDone)
                {
                    //把缓存的回调都清除
                    if (cacheDependLoaded != null)
                    {
                        cacheDependLoaded?.Invoke();
                        cacheDependLoaded = null;
                    }

                    if (loadState == LoadState.Loading)
                    {
                        assetBundle = assetBundleCreateRequest.assetBundle;
                        loadState = LoadState.Loaded;
                    }
                    else
                    {
                        Debug.LogError($"加载状态异常，请检查整个加载流程。loadState {loadState}");
                    }
                }
            }
        }


        public void UnloadAllLoadedObjects()
        {
            if (referenceCount == 1)
            {
                referenceCount = 0;

                foreach (var assetBundleLoadDepend in assetBundleLoadDependencies)
                {
                    assetBundleLoadDepend.UnloadAllLoadedObjects();
                }

                loadState = LoadState.Unload;
                assetBundle.Unload(true);
                assetBundle = null;
                loadedObjects.Clear();
            }
            else
            {
                referenceCount--;
            }
        }


        private Object LoadAsset(string assetName, System.Type type)
        {
            if (loadedObjects.TryGetValue(assetName, out Object asset) == false)
            {
                asset = assetBundle.LoadAsset(assetName, type);
                loadedObjects.Add(assetName, asset);
            }
            return asset;
        }
    }
}