using UnityEngine;
using UnityEditor;

namespace Cofdream.AssetEditor
{
    public sealed class AllFolderBuildRule : ScriptableObject, IBuildRule
    {
        public Object AssetFolder;

        public void CreateAssetBundleBuild(CreateCallback createCallback)
        {
            string path = AssetDatabase.GetAssetPath(AssetFolder);

            var folders = AssetDatabase.GetSubFolders(path);

            for (int i = 0; i < folders.Length; i++)
            {
                createCallback(new AssetBundleBuild()
                {
                    assetBundleName = BuildRuleUtil.PathToAssetBundleName(folders[i]),
                    assetNames = new string[] { folders[i] },
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
