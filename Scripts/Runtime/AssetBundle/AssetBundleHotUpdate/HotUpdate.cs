using Cofdream.AssetBuild;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cofdream
{
    public class HotUpdate : MonoBehaviour
    {
        public bool HasNewVersionRes;

        private string localAssetBundleVersionPath;

        private void Awake()
        {
            localAssetBundleVersionPath = Application.persistentDataPath + "/LocalAssetBundleVersion.json";

            AssetBundleBuildData assetBundleBuildData;
            if (File.Exists(localAssetBundleVersionPath) == false)
            {
                assetBundleBuildData = new AssetBundleBuildData();
                assetBundleBuildData.AssetBundleDatas = new AssetBundleData[0];
            }
            else
            {
                string json = File.ReadAllText(localAssetBundleVersionPath);
                assetBundleBuildData = JsonUtility.FromJson<AssetBundleBuildData>(json);
            }
           
            HotUpdateServer.GetHotUpdateAssetInfo(assetBundleBuildData,Application.platform);
        }

        public void HotUpedateAsset()
        {
            var remoteResVersion = 1;
            var localResVersion = 0;
            HasNewVersionRes = remoteResVersion > localResVersion;

            if (HasNewVersionRes)
            {
                UpdateAsset();
                Debug.Log("Update");
            }
            else
            {
                Debug.Log("Not Update");
            }

        }

        public void UpdateAsset()
        {
            // 下载需要更新的资源

            // 替换本地资源


        }

        private void Start()
        {

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