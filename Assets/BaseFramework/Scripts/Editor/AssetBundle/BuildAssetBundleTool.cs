using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cofdream.BaseFramework.UnityEngine;

namespace Cofdream.AssetEditor
{
    public class BuildAssetBundleTool
    {
        //BuildAssetBundleOptions.DisableWriteTypeTree
        /*
        在Unity 5.x版本中，AssetBundle在制作时会默认写入TypeTree信息，
        这样做的好处是可以保证AssetBundle文件的向下兼容性，即高版本可以支持以前低版本制作的AssetBundle文件。
        所以，如果开启DisableWriteTypeTree选项，则可能造成AssetBundle对Unity版本的兼容问题，
        虽然关闭TypeTree会使Bundle更小，但我们一般都不建议研发团队在制作AssetBundle文件时开启该选项。
        */


        /*
         *  None	                                    不使用任何特殊选项构建 assetBundle。
         *  UncompressedAssetBundle	                    创建资源包时不压缩数据。
         *  DisableWriteTypeTree	                    不包括 AssetBundle 中的类型信息。
         *  DeterministicAssetBundle	                使用存储在资源包中对象的 ID 的哈希构建资源包。
         *  ForceRebuildAssetBundle	                    强制重新构建 assetBundle。
         *  IgnoreTypeTreeChanges	                    在执行增量构建检查时忽略类型树更改。
         *  AppendHashToAssetBundleName	                向 assetBundle 名称附加哈希。
         *  ChunkBasedCompression	                    创建 AssetBundle 时使用基于语块的 LZ4 压缩。
         *  StrictMode	                                如果在此期间报告任何错误，则构建无法成功。
         *  DryRunBuild	                                进行干运行构建。
         *  DisableLoadAssetByFileName	                禁用按照文件名称查找资源包 LoadAsset。
         *  DisableLoadAssetByFileNameWithExtension	    禁用按照带扩展名的文件名称查找资源包 LoadAsset。
         *  AssetBundleStripUnityVersion	            在构建过程中删除存档文件和序列化文件头中的 Unity 版本号。
         */

        public static void BuildAssetBundle(BuildConfig buildConfig)
        {
            // ab保存路径
            var saveFolder = AssetUtil.GetAssetSaveFolderName(buildConfig.BuildPlatform);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Application.dataPath);
            stringBuilder.Remove(stringBuilder.Length - 6, 6);
            stringBuilder.Append("BuildAssetBundle/");
            stringBuilder.Append(saveFolder);

            var outputPath = stringBuilder.ToString();

            if (Directory.Exists(outputPath) == false) Directory.CreateDirectory(outputPath);


            // 获取 AssetBundleBuild
            var buildRuleFolders = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { buildConfig.RulesRootPath });

            int length = buildRuleFolders.Length;
            var buildRules = new List<IBuildRule>(length);
            for (int i = 0; i < length; i++)
            {
                var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(buildRuleFolders[i]));
                if (obj is IBuildRule buildRule)
                {
                    buildRules.Add(buildRule);
                }
                else
                    Debug.LogError($"对象不继承{typeof(IBuildRule)}请检查资源,{buildRuleFolders[i]}");
            }


            var assetBundleBuilds = new List<AssetBundleBuild>(50);
            foreach (var bundleRule in buildRules)
            {
                bundleRule.CreateAssetBundleBuild(
                    (AssetBundleBuild assetBundleBuild) =>
                    {
                        assetBundleBuilds.Add(assetBundleBuild);
                    });
            }


            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, assetBundleBuilds.ToArray(), buildConfig.BuildOptions, buildConfig.BuildPlatform);
                //  BuildAssetBundleOptions.ChunkBasedCompression
                //| BuildAssetBundleOptions.DisableWriteTypeTree
                //| BuildAssetBundleOptions.DeterministicAssetBundle
                ////| BuildAssetBundleOptions.StrictMode
                //, EditorUserBuildSettings.activeBuildTarget);


            // 记录打包信息
            //AssetBundleBuildData assetBundleBuildData = new AssetBundleBuildData();

            ////获取当前打包信息
            //var fileName = Path.GetFileNameWithoutExtension(outputPath);
            //var allABName = assetBundleManifest.GetAllAssetBundles();
            //assetBundleBuildData.AssetBundleDatas = new AssetBundleData[allABName.Length];

            //for (int i = 0; i < allABName.Length; i++)
            //{
            //    var assetBundleData = assetBundleBuildData.AssetBundleDatas[i] = new AssetBundleData();
            //    assetBundleData.AssetBundleName = allABName[i];
            //    assetBundleData.StringHash128 = assetBundleManifest.GetAssetBundleHash(assetBundleData.AssetBundleName).ToString();

            //    assetBundleData.Size = new FileInfo($"{outputPath}/{assetBundleData.AssetBundleName}").Length;
            //}

            //// 增加打包的版本
            //if (buildConfig == null)
            //    //build以后，之前加载的对象需要重新加载一下
            //    buildConfig = AssetDatabase.LoadAssetAtPath<BuildConfig>(configPath);

            //var assetBundleVersion = buildConfig.BuildAssetBundleVersions;
            //assetBundleVersion.Add();
            //EditorUtility.SetDirty(buildConfig);
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(buildConfig));

            //assetBundleBuildData.AssetBundleVersion = new BuildVersion() { version = assetBundleVersion.version };


            //var json = JsonUtility.ToJson(assetBundleBuildData);
            //File.WriteAllText(outputPath + "/AssetBundleBuildData.json", json);
            //File.WriteAllText(outputPath + "/AssetBundleBuildData.Read.json", JsonUtility.ToJson(assetBundleBuildData, true));
        }
    }
}