//using Cofdream.AssetLoad;
using Cofdream;
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
        AssetBundle.UnloadAllAssetBundles(true);

        AssetBundle assetBundle = AssetBundle.LoadFromFile(@"E:\Git\UnityBaseFramework\BuildAssetBundle\StandaloneWindows64\StandaloneWindows64");
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        Debug.Log(manifest.GetAssetBundleHash("assets_resource_headicon"));

        AssetBundle.UnloadAllAssetBundles(true);
    }

    void Update()
    {

    }

    private void OnDestroy()
    {
        //assetsLoad.UnLoad();
    }
}
