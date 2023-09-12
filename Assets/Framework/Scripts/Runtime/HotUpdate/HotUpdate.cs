using Cofdream.BaseFramework.UnityEngine.AssetBundleBuild;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Cofdream.BaseFramework.UnityEngine.HotUpdate
{
    public class HotUpdate : MonoBehaviour
    {
        public bool HasNewVersionRes;

        private string localAssetBundleVersionPath;

        public Text longText;

        private void Awake()
        {
            localAssetBundleVersionPath = @"C:\Users\v_cqqcchen\Desktop\Test/LocalAssetBundleVersion.json";

            AssetBundleBuildData assetBundleBuildData;
            if (File.Exists(localAssetBundleVersionPath) == false)
            {
                assetBundleBuildData = new AssetBundleBuildData();
                assetBundleBuildData.AssetBundleDatas = new AssetBundleData[0];
                assetBundleBuildData.AssetBundleVersion = new BuildVersion();
            }
            else
            {
                string json = File.ReadAllText(localAssetBundleVersionPath);
                assetBundleBuildData = JsonUtility.FromJson<AssetBundleBuildData>(json);
            }

            long updateSize = HotUpdateServer.I.GetHotUpdateAssetInfo(assetBundleBuildData, Application.platform);

            if (updateSize > 0)
            {
                longText.gameObject.SetActive(true);
                longText.text = GetFileSize(updateSize);
            }
        }
        public void UpdateAsset()
        {
            longText.gameObject.SetActive(false);
            // 下载需要更新的资源

            // 替换本地资源
        }

        private void Start()
        {

        }

        private const float OneKB = 1024;
        private const float OneMB = 1048576;      // System.Math.Pow(OneKB, 2);
        private const float OneGB = 1073741824;   // System.Math.Pow(OneKB, 3);
        private const float OneTB = 1099511627776;// System.Math.Pow(OneKB, 4);
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string GetFileSize(long size)
        {
            if (size < OneKB) return size + "B";
            if (size < OneMB) return (size / OneKB).ToString("f2") + "K";
            if (size < OneGB) return (size / OneMB).ToString("f2") + "M";
            if (size < OneTB) return (size / OneGB).ToString("f2") + "G";

            return (size / OneTB).ToString("f2") + "T";
        }
    }

    /*
    打包
    build ab 
    累加版本 



    本地
    资源更新
    请求 资源更新
    发送本地版本

    
    库
    收到 资源更新请求 获取库最新版本 获取请求的版本
    返回
        是否需要更新资源
        需要更新的资源大小

    本地
    需要更新资源
    提示是否开始更新
    是
    开始更新资源 显示下载进度

    结束资源跟香港
        
     */
}