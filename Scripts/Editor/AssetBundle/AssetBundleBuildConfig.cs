using Cofdream.AssetBuild;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CofdreamEditor.AssetBundle
{
    public class AssetBundleBuildConfig : ScriptableObject
    {
        [SerializeField] DefaultAsset[] rulesFolders;

        [SerializeField] BuildTarget[] buildPlatforms;
        [SerializeField] AssetBundleVersion[] buildAssetBundleVersions;

        public string[] GetRulesFolders()
        {
            string[] folders = new string[rulesFolders.Length];
            for (int i = 0; i < rulesFolders.Length; i++)
            {
                string rulesFoldPath = AssetDatabase.GetAssetPath(rulesFolders[i]);
                if (AssetDatabase.IsValidFolder(rulesFoldPath))
                {
                    folders[i] = rulesFoldPath;
                }
            }
            return folders;
        }

        public AssetBundleVersion GetCurrentBuildAssetBundleVersion(BuildTarget buildPlatform)
        {
            int index = buildPlatforms.Length;
            for (int i = 0; i < buildPlatforms.Length; i++)
            {
                if (buildPlatforms[i] == buildPlatform)
                {
                    index = i;
                    break;
                }
            }

            if (index == buildPlatforms.Length)
            {
                int length = index + 1;

                BuildTarget[] newBuildPlatforms = new BuildTarget[length];
                buildPlatforms.CopyTo(newBuildPlatforms, index);
                buildPlatforms = newBuildPlatforms;
                buildPlatforms[index] = buildPlatform;

                AssetBundleVersion[] newBuildAssetBundleVersions = new AssetBundleVersion[length];
                buildAssetBundleVersions.CopyTo(newBuildAssetBundleVersions, index);
                buildAssetBundleVersions = newBuildAssetBundleVersions;
                buildAssetBundleVersions[index] = new AssetBundleVersion();

                EditorUtility.SetDirty(this);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
            }

            return buildAssetBundleVersions[index];
        }

        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
        }

        [CustomEditor(typeof(AssetBundleBuildConfig))]
        public class AssetBundleBuildConfigInspector : Editor
        {

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                var assetBundleBuildConfig = target as AssetBundleBuildConfig;

                for (int i = 0; i < assetBundleBuildConfig.rulesFolders.Length; i++)
                {
                    var folder = assetBundleBuildConfig.rulesFolders[i];

                    if (folder == null)
                    {
                        DrawBox();
                        return;
                    }
                    string rulesFoldPath = AssetDatabase.GetAssetPath(folder);
                    if (AssetDatabase.IsValidFolder(rulesFoldPath) == false)
                    {
                        DrawBox();
                        assetBundleBuildConfig.rulesFolders[i] = null;
                        return;
                    }
                }

            }

            private void DrawBox()
            {
                EditorGUILayout.HelpBox("RulesFolders exist error folder value.", MessageType.Error);
            }
        }
    }
}