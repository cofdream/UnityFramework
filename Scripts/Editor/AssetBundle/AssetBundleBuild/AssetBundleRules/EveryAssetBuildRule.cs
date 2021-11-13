using UnityEngine;
using UnityEditor;

namespace Cofdream.AssetEditor
{
    /// <summary>
    /// 文件夹目录下 每个资源文件都打单独AB包
    /// </summary>
    public class EveryAssetBuildRule : ScriptableObject, IBuildRule
    {
        public DefaultAsset AssetFolder;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            string path = AssetDatabase.GetAssetPath(AssetFolder);
            var folders = AssetDatabase.GetSubFolders(path);

            for (int i = 0; i < folders.Length; i++)
            {
                createCallback(new AssetBundleBuild()
                {
                    assetBundleName = folders[i].Replace('/', '_'),
                    assetNames = new string[] { folders[i] },
                });
            }

            var files = System.IO.Directory.GetFiles(path, "*.meta");
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string assetPath = file.Substring(0, file.Length - 5);
                string assetBundleName = assetPath.Replace('/', '_');

                createCallback(new AssetBundleBuild()
                {
                    assetBundleName = assetBundleName,
                    assetNames = new string[] { assetPath },
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