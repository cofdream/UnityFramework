using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CofdreamEditor.AssetBuild
{
    public static class BuildRuleUtil
    {
        private const string spacing = "_";
        private static readonly Regex regex = new Regex("\\\\");
        private static readonly Regex regex2 = new Regex("/");

        public static string PathToAssetBundleName(string path)
        {
            return regex2.Replace(regex.Replace(path, "/"), spacing);
        }
    }
}
