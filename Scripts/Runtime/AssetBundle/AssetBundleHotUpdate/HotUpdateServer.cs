using Cofdream.AssetBuild;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    List<int> addList = new List<int>();
    List<int> changeList = new List<int>();
    List<int> checkList = new List<int>();

    public long GetHotUpdateAssetInfo(AssetBundleBuildData assetBundleBuildData, RuntimePlatform platform)
    {
        //根据平台，获取最新的ab数据
        string rootPath = @"E:\Git\UnityBaseFramework\BuildAssetBundle\StandaloneWindows64";
        AssetBundleBuildData assetBundleBuildDataServer = JsonUtility.FromJson<AssetBundleBuildData>(File.ReadAllText(rootPath + "/AssetBundleBuildData.json"));

        int temp = assetBundleBuildDataServer.AssetBundleVersion.CompareTo(assetBundleBuildData.AssetBundleVersion);
        Debug.Log(temp);

        if (temp > 0)
        {
            int clientAssetBundleDataCount = assetBundleBuildData.AssetBundleDatas.Length;
            Dictionary<string, int> clientAssetBundleDataDic = new Dictionary<string, int>(clientAssetBundleDataCount);
            for (int i = 0; i < clientAssetBundleDataCount; i++)
            {
                clientAssetBundleDataDic.Add(assetBundleBuildData.AssetBundleDatas[i].AssetBundleName, i);
            }

            addList.Clear();
            changeList.Clear();
            checkList.Clear();

            long changeSize = 0;

            for (int i = 0; i < assetBundleBuildDataServer.AssetBundleDatas.Length; i++)
            {
                var assetBundleDataServer = assetBundleBuildDataServer.AssetBundleDatas[i];
                if (clientAssetBundleDataDic.TryGetValue(assetBundleDataServer.AssetBundleName, out int index))
                {
                    AssetBundleData assetBundleDataClient = assetBundleBuildData.AssetBundleDatas[index];
                    if (assetBundleDataClient.Size == assetBundleDataServer.Size && assetBundleDataClient.StringHash128 == assetBundleDataServer.StringHash128)
                    {

                    }
                    else
                    {
                        changeSize += assetBundleDataServer.Size;
                        changeList.Add(i);
                        Debug.Log("change " + assetBundleDataServer.AssetBundleName);
                    }
                }
                else
                {
                    changeSize += assetBundleDataServer.Size;
                    addList.Add(i);
                    Debug.Log("Add " + assetBundleDataServer.AssetBundleName);
                }
                checkList.Add(index);
            }

            // 可以计算需要删除的文件
            checkList.Sort();

            //int removeCount = clientAssetBundleDataCount - checkList.Count;
            //List<int> removeList = new List<int>(removeCount);
            //for (int i = 0; i < removeCount; i++)
            //{

            //}

            return changeSize;
        }
        else
        {
            return 0;
        }

        //比较
        /*
           收到 资源更新请求 获取库最新版本 获取请求的版本
    返回
        是否需要更新资源
        需要更新的资源大小
         */
    }
}