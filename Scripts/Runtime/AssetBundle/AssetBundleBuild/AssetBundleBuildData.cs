using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.AssetBuild
{
    [System.Serializable]
    public class AssetBundleBuildData
    {
        public AssetBundleData[] AssetBundleDatas;
        public BuildVersion AssetBundleVersion;
    }

    [System.Serializable]
    public class AssetBundleData
    {
        public string AssetBundleName;

        public string StringHash128;
        public long Size;
    }

    [System.Serializable]
    public class BuildVersion
    {
        public uint version;

        public void Add()
        {
            version++;
        }

        public int CompareTo(BuildVersion assetBuildVersion)
        {
            return version.CompareTo(assetBuildVersion.version);
        }
    }
}
