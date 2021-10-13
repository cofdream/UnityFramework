using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Cofdream.AssetBuild;

namespace CofdreamEditor.AssetBuild
{
    public class AssetBundleBuildKit
    {
        [MenuItem("Asset Bundle BuildKit/Asset Bundle Build")]
        static void AssetBundleBuild()
        {
            string outputPath = Application.dataPath;
            outputPath = outputPath.Remove(outputPath.Length - 6);

            outputPath += @"BuildAssetBundle\Windows\";


            var buildRuleFolders = AssetDatabase.FindAssets(string.Empty, new string[] { "Assets/Cofdream/AssetBundleBuild/Rules" });

            List<IBuildRule> buildRules = new List<IBuildRule>(buildRuleFolders.Length);
            for (int i = 0; i < buildRuleFolders.Length; i++)
            {
                var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(buildRuleFolders[i]));
                IBuildRule buildRule = obj as IBuildRule;
                if (buildRule == null)
                {
                    Debug.LogError($"对象不继承{typeof(IBuildRule)}请检查资源,{buildRuleFolders[i]}");
                    continue;
                }

                buildRules.Add(buildRule);
            }
            
            assetBundleBuilds = new List<AssetBundleBuild>(50);

            foreach (var bundleRule in buildRules)
            {
                bundleRule.CreateAssetBundleBuild(AssetBundleBuildCallBack);
            }

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, assetBundleBuilds.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.DeterministicAssetBundle |
                BuildAssetBundleOptions.StrictMode, EditorUserBuildSettings.activeBuildTarget);

            

            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
            //Debug.ClearDeveloperConsole();

            var allABName = assetBundleManifest.GetAllAssetBundles();

            Cofdream.AssetBuild.AssetBundleManifest[] assetBundleManifests = new Cofdream.AssetBuild.AssetBundleManifest[allABName.Length];


            for (int i = 0; i < allABName.Length; i++)
            {
                var bundleManifest = assetBundleManifests[i] = new Cofdream.AssetBuild.AssetBundleManifest();
                bundleManifest.AssetBundleName = allABName[i];

                bundleManifest.Hash = assetBundleManifest.GetAssetBundleHash(bundleManifest.AssetBundleName);
                bundleManifest.Size = new FileInfo($"{outputPath}/{bundleManifest.AssetBundleName}").Length;

                Debug.Log(allABName[i] + "__" + bundleManifest.Hash + "__" + bundleManifest.Size);
            }

            AssetBundleManifestArray assetBundleManifestArray = new AssetBundleManifestArray();
            assetBundleManifestArray.AssetBundleManifests = assetBundleManifests;

            var json = JsonUtility.ToJson(assetBundleManifestArray);
            File.WriteAllText(outputPath + "/AssetBundleManifest.json", json);

            // todo hotUpdate
            // 下载服务器 ab目录 比较本地 ab目录
            // 不再使用的ab文件删除？
        }



        private static List<AssetBundleBuild> assetBundleBuilds;
        private static void AssetBundleBuildCallBack(AssetBundleBuild assetBundleBuild)
        {
            assetBundleBuilds.Add(assetBundleBuild);
        }
    }
}