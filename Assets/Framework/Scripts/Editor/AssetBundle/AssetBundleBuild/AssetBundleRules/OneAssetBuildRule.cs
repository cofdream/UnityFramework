using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.AssetEditor
{
    /// <summary>
    /// 只打包当前的Asset
    /// </summary>
    public class OneAssetBuildRule : ScriptableObject, IBuildRule
    {
        public Object asset;

        public string assetBundleName;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            string assetBundleName;
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                assetBundleName = assetPath.Replace('/', '_');
            }
            else
            {
                assetBundleName = assetPath.Replace('/', '_').Replace('.', '_');
            }
            createCallback(new AssetBundleBuild()
            {
                assetBundleName = assetBundleName,
                assetNames = new string[] { assetPath },
            });
        }
       
        private void OnValidate()
        {
            if (asset != null)
            {
                var assetPath = AssetDatabase.GetAssetPath(asset);
                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    assetBundleName = assetPath.Replace('/', '_');
                }
                else
                {
                    assetBundleName = assetPath.Replace('/', '_').Replace('.', '_');
                }
            }
        }
    }
}