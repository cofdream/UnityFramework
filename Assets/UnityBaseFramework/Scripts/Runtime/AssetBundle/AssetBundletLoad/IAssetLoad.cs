using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetLoad
{
    public interface IAssetLoad
    {
        public Object LoadAsset(string assetName, System.Type type);
        public void UnAllLoad();
    }
}
