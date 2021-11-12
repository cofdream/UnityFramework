using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.AssetEditor
{
    /// <summary>
    /// 自定义打AB包，内容自选。
    /// </summary>
    public class CustomBuildRule : ScriptableObject, IBuildRule
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