using Cofdream.AssetBuild;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.AssetEditor
{
    public class BuildConfig : ScriptableObject
    {
        //[SerializeField, TextArea(3, 6)] string description;

        [SerializeField] DefaultAsset rulesFolder;
        [SerializeField] BuildTarget buildPlatforms;
        [SerializeField] BuildVersion BuildVersion;
        [SerializeField] BuildAssetBundleOptions buildOptions;

        public string RulesRootPath => AssetDatabase.GetAssetPath(rulesFolder);
        public BuildTarget BuildPlatform => buildPlatforms;
        public BuildVersion BuildAssetBundleVersions => BuildVersion;
        public BuildAssetBundleOptions BuildOptions => buildOptions;

        //[CustomEditor(typeof(BuildConfig))]
        //public class AssetBundleBuildConfigInspector : Editor
        //{

        //    public override void OnInspectorGUI()
        //    {
        //        base.OnInspectorGUI();

        //        var assetBundleBuildConfig = target as BuildConfig;

        //        //for (int i = 0; i < assetBundleBuildConfig.rulesFolder.Length; i++)
        //        //{
        //        //    var folder = assetBundleBuildConfig.rulesFolder[i];

        //        //    if (folder == null)
        //        //    {
        //        //        DrawBox();
        //        //        return;
        //        //    }
        //        //    string rulesFoldPath = AssetDatabase.GetAssetPath(folder);
        //        //    if (AssetDatabase.IsValidFolder(rulesFoldPath) == false)
        //        //    {
        //        //        DrawBox();
        //        //        assetBundleBuildConfig.rulesFolder[i] = null;
        //        //        return;
        //        //    }
        //        //}
        //    }

        //    private void DrawBox()
        //    {
        //        EditorGUILayout.HelpBox("RulesFolders exist error folder value.", MessageType.Error);
        //    }
        //}


        [CustomEditor(typeof(BuildConfig)), CanEditMultipleObjects]
        private class BuildConfigInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();


                if (targets.Length == 1)
                {
                    EditorGUILayout.Space(40);

                    if (GUILayout.Button(" Build ", GUILayout.ExpandWidth(false)))
                    {
                        var buildConfig = target as BuildConfig;
                        if (buildConfig != null)
                        {
                            BuildAssetBundleTool.BuildAssetBundle(buildConfig);
                        }
                    }
                }
            }
        }
    }
}