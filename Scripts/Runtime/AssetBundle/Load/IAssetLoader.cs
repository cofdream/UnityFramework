using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cofdream.Asset
{
    public interface IAssetLoader
    {
        Object Load(string assetName, System.Type type);
        void LoadAsync(UnityAction<IAssetLoader> loaded);
        void UnloadAllLoadedObjects();
    }
}
