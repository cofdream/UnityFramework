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

    [CustomEditor(typeof(CustomBuildRule))]
    [CanEditMultipleObjects]
    public class CustomBuildRuleInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("包名 / \\ 转换成 _"))
            {
                foreach (var item in targets)
                {
                    var obj = (CustomBuildRule)item;
                    obj.AssetBundleName = obj.AssetBundleName.Replace('/', '_').Replace('\\', '_');
                }
            }
            if (GUILayout.Button("资源名数组 / \\ 转换成 _"))
            {
                foreach (var item in targets)
                {
                    var obj = (CustomBuildRule)item;
                    int length = obj.AssetNames.Length;
                    for (int i = 0; i < length; i++)
                    {
                        obj.AssetNames[i] = obj.AssetNames[i].Replace('/', '_').Replace('\\', '_');
                    }
                }
            }
        }
    }
}