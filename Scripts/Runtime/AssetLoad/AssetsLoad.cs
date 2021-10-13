using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetLoad
{
    public class AssetsLoad
    {
        public static readonly System.Type Pre = typeof(GameObject);
        public static readonly System.Type Mat = typeof(Material);
        public static readonly System.Type Tex = typeof(Texture2D);
        public static readonly System.Type Spr = typeof(Sprite);
        public static readonly System.Type Txt = typeof(TextAsset);

        private AssetBundleLoad assetBundleLoad;
        private EditorAssetLoad editorAssetLoad;
        public AssetsLoad(string assetBundleName)
        {
            if (EditorAssetLoad.LocalLoadModel)
            {
                editorAssetLoad = EditorAssetLoad.Take(assetBundleName);
            }
            else
            {
                assetBundleLoad = AssetBundleLoad.Take(assetBundleName);
            }
        }

        public T Load<T>(string assetName) where T : Object
        {
            return (T)Load(assetName, typeof(T));
        }

        public Object Load(string assetName, System.Type type)
        {
            if (EditorAssetLoad.LocalLoadModel)
            {
                return editorAssetLoad.LoadAsset(assetName, type);
            }
            else
            {
                return assetBundleLoad.LoadAsset(assetName, type);
            }
        }

        public void UnLoad()
        {
            if (EditorAssetLoad.LocalLoadModel)
            {
                EditorAssetLoad.Put(editorAssetLoad);
                editorAssetLoad = null;
            }
            else
            {
                AssetBundleLoad.Put(assetBundleLoad);
                assetBundleLoad = null;
            }
        }
    }
}