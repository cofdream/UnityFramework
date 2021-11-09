using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.Asset
{
    public static class AssetsLoad
    {
        public static IAssetLoader GetAssetLoad(string assetBundleName)
        {
#if UNITY_EDITOR
            if (EditorAssetLoader.AssetBundleLoad)
            {
                return EditorAssetLoader.GetLoader(assetBundleName);
            }
#endif
            return AssetBundleLoader.GetLoader(assetBundleName);
        }

        public static T Load<T>(this IAssetLoader assetLoad, string assetName) where T : Object
        {
            return (T)Load(assetLoad, assetName, typeof(T));
        }

        public static Object Load(this IAssetLoader assetLoad, string assetName, System.Type type)
        {
            return assetLoad.Load(assetName, type);
        }

        public static void UnLoad(this IAssetLoader assetLoad)
        {
            assetLoad.UnloadAllLoadedObjects();
        }
    }
}