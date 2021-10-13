using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetLoad
{
    public class AssetBundleLoad
    {
        private uint referenceCount;
        private LoadState loadState;
        private string assetBundleName;

        public AssetBundle assetBundle;
        private AssetBundleLoad[] assetBundleLoadDependencies;

        private Dictionary<string, Object> allAsset;


        private static Dictionary<string, AssetBundleLoad> allAssetBundleDic = new Dictionary<string, AssetBundleLoad>();

        public static AssetBundleLoad Take(string assetBundleName)
        {
            AssetBundleLoad assetBundleLoad;
            if (allAssetBundleDic.TryGetValue(assetBundleName, out assetBundleLoad) == false)
            {
                //todo 验证是ab包名正确性 

                assetBundleLoad = new AssetBundleLoad();
                allAssetBundleDic.Add(assetBundleName, assetBundleLoad);

                assetBundleLoad.referenceCount = 0;
                assetBundleLoad.loadState = LoadState.NotLoad;
                assetBundleLoad.assetBundleName = assetBundleName;
                assetBundleLoad.allAsset = new Dictionary<string, Object>();

                assetBundleLoad.referenceCount++;
                return assetBundleLoad;
            }
            else
            {
                assetBundleLoad.referenceCount++;
                return assetBundleLoad;
            }
        }

        public static void Put(AssetBundleLoad assetBundleLoad)
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
                assetBundleLoad.allAsset.Clear();
            }
            else
            {
                assetBundleLoad.referenceCount--;
            }
        }


        public void Load()
        {
            switch (loadState)
            {
                case LoadState.NotLoad:
                case LoadState.Unload:

                    assetBundle = AssetBundle.LoadFromFile(@"E:\Git\Null\Project\BuildAssetBundle\Windows\" + assetBundleName);

                    //todo cache
                    var assetBundleMain = AssetBundle.LoadFromFile(@"E:\Git\Null\Project\BuildAssetBundle\Windows\Windows");
                    var assetBundleManifest = assetBundleMain.LoadAsset<UnityEngine.AssetBundleManifest>("AssetBundleManifest");

                    var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
                    if (dependencies.Length > 0)
                    {
                        assetBundleLoadDependencies = new AssetBundleLoad[dependencies.Length];
                    }
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        var assetBundleLoad = Take(dependencies[i]);
                        assetBundleLoad.Load();
                        assetBundleLoadDependencies[i] = assetBundleLoad;
                    }

                    assetBundleMain.Unload(true);


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
        public void LoadAsync()
        {
            //switch (loadState)
            //{
            //    case LoadState.NotLoad:
            //    case LoadState.Unload:

            //        assetBundle = AssetBundle.LoadFromFileAsync(assetBundleName);

            //        //todo cache
            //        var assetBundleMain = AssetBundle.LoadFromFile(@"E:\Git\Null\Project\BuildAssetBundle\Windows");
            //        var assetBundleManifest = assetBundleMain.LoadAsset<UnityEngine.AssetBundleManifest>("AssetBundleManifest");

            //        var dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
            //        for (int i = 0; i < dependencies.Length; i++)
            //        {
            //            var assetBundleLoad = Take(dependencies[i]);
            //            assetBundleLoad.Load();
            //            dependenciesLoad[i] = assetBundleLoad;
            //        }

            //        assetBundleMain.Unload(true);


            //        if (assetBundle == null)
            //        {
            //            loadState = LoadState.LoadError;
            //        }
            //        else
            //        {
            //            loadState = LoadState.Loaded;
            //        }

            //        break;
            //    case LoadState.Loading:
            //        // todo 同步加载的ab/依赖ab 再加载中
            //        break;
            //    case LoadState.Loaded:
            //        break;
            //    case LoadState.LoadError:
            //        break;
            //}
        }


        public Object LoadAsset(string assetName, System.Type type)
        {
            Load();

            Object asset;
            if (allAsset.TryGetValue(assetName, out asset) == false)
            {
                asset = assetBundle.LoadAsset(assetName, type);
                allAsset.Add(assetName, asset);
            }
            return asset;
        }

    }
}
