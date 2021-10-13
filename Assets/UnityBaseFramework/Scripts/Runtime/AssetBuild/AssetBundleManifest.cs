using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetBuild
{
    [System.Serializable]
    public class AssetBundleManifestArray
    {
        public AssetBundleManifest[] AssetBundleManifests;
    }

    [System.Serializable]
    public class AssetBundleManifest
    {
        public string AssetBundleName;

        public Hash128 Hash;
        public long Size;
    }
}
