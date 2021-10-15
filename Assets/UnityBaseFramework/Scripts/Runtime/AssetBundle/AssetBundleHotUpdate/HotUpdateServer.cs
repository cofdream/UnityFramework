using Cofdream.AssetBuild;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotUpdateServer : MonoBehaviour
{
    private static HotUpdateServer instance;
    public static HotUpdateServer I => instance;

    static HotUpdateServer()
    {
        instance = new GameObject("HotUpdateServer").AddComponent<HotUpdateServer>();
    }

    void Update()
    {

    }

    public static void GetHotUpdateAssetInfo(AssetBundleBuildData assetBundleBuildData, RuntimePlatform platform)
    {
        string rootPath = @"E:\Git\UnityBaseFramework\BuildAssetBundle\StandaloneWindows64";

        AssetBundleBuildData assetBundleBuildDataServer = JsonUtility.FromJson<AssetBundleBuildData>(rootPath + "/AssetBundleBuildData.json");

        //比较
        /*
           收到 资源更新请求 获取库最新版本 获取请求的版本
    返回
        是否需要更新资源
        需要更新的资源大小
         */
    }

}

