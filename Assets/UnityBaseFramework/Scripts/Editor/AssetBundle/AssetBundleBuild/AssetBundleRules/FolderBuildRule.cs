using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.AssetEditor
{
    public sealed class FolderBuildRule : ScriptableObject, IBuildRule
    {
        public Object AssetFolder;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            string path = AssetDatabase.GetAssetPath(AssetFolder);

            createCallback(new AssetBundleBuild()
            {
                assetBundleName = BuildRuleUtil.PathToAssetBundleName(path),
                assetNames = new string[] { path },
            });
        }

        private void OnValidate()
        {
            if (AssetFolder != null)
            {
                string path = AssetDatabase.GetAssetPath(AssetFolder);
                if (AssetDatabase.IsValidFolder(path) == false)
                {
                    EditorUtility.DisplayDialog("警告", "不是文件夹路径", "确认");
                }
            }
        }
    }
}