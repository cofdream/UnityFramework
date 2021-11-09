using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.Asset
{
#if UNITY_EDITOR
    public static partial class AssetUtil
    {
        public static string GetAssetSaveFolderName(UnityEditor.BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return "Win";
                case UnityEditor.BuildTarget.iOS:
                    return "IOS";
                case UnityEditor.BuildTarget.Android:
                    return "And";
                default:
                    return "Oth";
            }
        }
    }
#endif
}