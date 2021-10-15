using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetLoad
{
    public static class AssetsLoad
    {
        public static readonly System.Type Pre = typeof(GameObject);
        public static readonly System.Type Mat = typeof(Material);
        public static readonly System.Type Tex = typeof(Texture2D);
        public static readonly System.Type Spr = typeof(Sprite);
        public static readonly System.Type Txt = typeof(TextAsset);


        public static IAssetLoad GetAssetLoad(string assetBundleName)
        {
#if UNITY_EDITOR
            if (EditorAssetLoad.LocalLoadModel)
            {
                return EditorAssetLoad.Take(assetBundleName);
            }
#endif
            return AssetBundleLoad.Take(assetBundleName);
        }

        public static T Load<T>(this IAssetLoad assetLoad, string assetName) where T : Object
        {
            return (T)Load(assetLoad, assetName, typeof(T));
        }

        public static Object Load(this IAssetLoad assetLoad, string assetName, System.Type type)
        {
            return assetLoad.LoadAsset(assetName, type);
        }

        public static void UnLoad(this IAssetLoad assetLoad)
        {
            assetLoad.UnAllLoad();
        }
    }
}