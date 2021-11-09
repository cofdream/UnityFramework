using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.AssetEditor
{
    internal class UnityCommmand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
        private static void BuildAssetBundle()
        {
            var args = System.Environment.GetCommandLineArgs();
            string build = "-build";
            string win = "Win";
            string and = "And";
            string ios = "Ios";

            string path = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith(build))
                {
                    if (arg.EndsWith(win))
                    {
                        path = "Assets/Resource/AssetBundle/BuildConfigWindow.asset";
                    }
                    else if (arg.EndsWith(and))
                    {
                        path = "Assets/Resource/AssetBundle/BuildConfigAndroid.asset";
                    }
                    else if (arg.EndsWith(ios))
                    {
                        path = "Assets/Resource/AssetBundle/BuildConfigIos.asset";
                    }
                }
            }
            if (path == null)
            {
                Debug.LogError("打AB失败，命令行参数错误");
                return;
            }
            var buildConfig = AssetDatabase.LoadAssetAtPath<BuildConfig>(path);
            if (buildConfig == null)
            {
                Debug.LogError("打AB失败，配置文件路径错误，检查路径：" + path);
                return;
            }
            BuildAssetBundleTool.BuildAssetBundle(buildConfig);
        }

        private static string[] GetBuildArgs()
        {
            var args = System.Environment.GetCommandLineArgs();
            var argHead = "-buildArg";
            var argLength = argHead.Length;
            var argList = new List<string>();

            int length = args.Length;
            for (int i = 0; i < length; i++)
            {
                var arg = args[i];

                if (arg.StartsWith(argHead))
                {
                    if (arg.Length == argLength)
                    {
                        // 读下一行
                        if (i++ < length)
                        {
                            argList.Add(args[i]);
                        }
                        else
                            break;
                    }
                    else
                    {
                        argList.Add(arg.Remove(0, argLength));
                    }
                }
            }

            return argList.ToArray();
        }
    }
}
