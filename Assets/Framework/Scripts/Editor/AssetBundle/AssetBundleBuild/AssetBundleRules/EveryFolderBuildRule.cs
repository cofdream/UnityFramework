using UnityEngine;
using UnityEditor;

namespace Cofdream.AssetEditor
{
    /// <summary>
    /// 目录下全部文件夹打单独AB包（只打包文件夹）
    /// </summary>
    public sealed class EveryFolderBuildRule : ScriptableObject, IBuildRule
    {
        public DefaultAsset AssetFolder;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            string path = AssetDatabase.GetAssetPath(AssetFolder);

            var folders = AssetDatabase.GetSubFolders(path);

            for (int i = 0; i < folders.Length; i++)
            {
                string folder = folders[i];
                createCallback(new AssetBundleBuild()
                {
                    assetBundleName = folder.Replace('/','_'),
                    assetNames = new string[] { folder },
                });
            }
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