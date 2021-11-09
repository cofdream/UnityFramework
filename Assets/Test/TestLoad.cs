using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    public string[] Paths;


    //private AssetsLoad assetsLoad;

    public List<Object> allAssets;

    private void Awake()
    {

    }

    void Start()
    {
        allAssets = new List<Object>();

        //assetsLoad = new AssetsLoad("assets_cofdream_resource_battlemap_10001");

        //allAssets.Add(assetsLoad.Load("Map_10001.prefab", typeof(GameObject)) as GameObject);
        //allAssets.Add(assetsLoad.Load<GameObject>("Map_10001.prefab"));

        //todo hot update

        //loader.Load<GameObject>("assets_cofdream_resource_battlemap_10001", "Map_10003.prefab");


        //AssetBundle.UnloadAllAssetBundles(true);

        //AssetBundle assetBundle = AssetBundle.LoadFromFile(@"E:\Git\UnityBaseFramework\BuildAssetBundle\StandaloneWindows64\StandaloneWindows64");
        //AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //Debug.Log(manifest.GetAssetBundleHash("assets_resource_headicon"));

        //AssetBundle.UnloadAllAssetBundles(true);
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        //assetsLoad.UnLoad();
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
