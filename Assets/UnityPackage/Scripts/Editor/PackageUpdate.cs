using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public static class PackageUpdate
    {
        [MenuItem("Package/Update")]
        public static void Run()
        {
            RunCommand();
            EditorUtility.ClearProgressBar();
        }
        private static void RunCommand()
        {
            var processCMD = new ProcessCommand();

            EditorUtility.DisplayProgressBar("Package更新", "拉取修改", 0);
            processCMD.Cmd = "git pull";
            if (!processCMD.Execute()) return;

            EditorUtility.DisplayProgressBar("Package更新", "主干修改合并upm分支", 0.2f);
            processCMD.Cmd = "git subtree split --rejoin --prefix=Assets/UnityBaseFramework --branch upm";
            if (!processCMD.Execute()) return;

            EditorUtility.DisplayProgressBar("Package更新", "提交修改", 0.8f);
            processCMD.Cmd = "git push origin upm";
            if (!processCMD.Execute()) return;
        }
    }
}