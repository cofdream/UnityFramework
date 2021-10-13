using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CofdreamEditor.AssetBuild
{
    public class BuildRule : ScriptableObject, IBuildRule
    {
        public string AssetBundleName;
        public string[] AssetNames;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            createCallback(new AssetBundleBuild()
            {
                assetBundleName = AssetBundleName,
                assetNames = AssetNames,
            });
        }
    }
}