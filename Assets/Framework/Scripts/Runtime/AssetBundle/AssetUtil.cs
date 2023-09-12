using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.BaseFramework.UnityEngine
{
    public static partial  class AssetUtil
    {
        public static string GetAssetSaveFolderName(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Win";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.Android:
                    return "And";
                default:
                    return "Oth";
            }
        }
    }
}
