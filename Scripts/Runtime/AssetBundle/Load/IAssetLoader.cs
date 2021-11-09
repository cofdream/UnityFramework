using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.Asset
{
    public interface IAssetLoader
    {
        public delegate void AssetLoaded(Object obj);

        public Object Load(string assetName, System.Type type);
        public void LoadAsync(string assetName, AssetLoaded assetLoaded);
        public void UnloadAllLoadedObjects();
    }
}
