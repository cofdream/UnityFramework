using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Cofdream.Asset
{
#if UNITY_EDITOR
    public class EditorAssetLoader : IAssetLoader
    {
        public static bool AssetBundleLoad { get; private set; }

        private const bool DefaultValue = true;
        private const string LOCAL_LOAD_MODEL = "EditorAssetLoad.AssetBundleLoad";
        private const string MENI_ITEM_NAME = "CAsset/AssetBundleLoad";
        private const int priority = int.MaxValue;

        static EditorAssetLoader()
        {
            AssetBundleLoad = EditorPrefs.GetBool(LOCAL_LOAD_MODEL, DefaultValue);
        }

        [MenuItem(MENI_ITEM_NAME, true, priority),]
        [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
        private static bool InitLocalLoadModel()
        {
            if (EditorApplication.isPlaying) return false;

            Menu.SetChecked(MENI_ITEM_NAME, AssetBundleLoad);
            return true;
        }

        [MenuItem(MENI_ITEM_NAME, false, priority)]
        [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
        private static void SwitchLocalLoadModel()
        {
            AssetBundleLoad = !AssetBundleLoad;
            EditorPrefs.SetBool(LOCAL_LOAD_MODEL, AssetBundleLoad);

            Menu.SetChecked(MENI_ITEM_NAME, AssetBundleLoad);
        }

        [MenuItem("CAsset/粘贴板斜杠替换为下划线", false, priority - 11)]
        [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
        private static void PathReplace()
        {
            GUIUtility.systemCopyBuffer = GUIUtility.systemCopyBuffer.Replace('/', '_');

            Resources.UnloadUnusedAssets();
            AssetBundle.UnloadAllAssetBundles(true);
        }


        private uint referenceCount;
        private string assetBundleName;
        private string[] allAssetPath;

        private Dictionary<string, Object> loadedObjects;

        private static Dictionary<string, EditorAssetLoader> allEditorAssetLoader = new Dictionary<string, EditorAssetLoader>();

        EditorAssetLoader() { }

        public static EditorAssetLoader GetLoader(string assetBundleName)
        {
            if (allEditorAssetLoader.TryGetValue(assetBundleName, out EditorAssetLoader editorAssetLoad) == false)
            {
                //todo 验证是ab包名正确性 

                editorAssetLoad = new EditorAssetLoader();
                allEditorAssetLoader.Add(assetBundleName, editorAssetLoad);

                editorAssetLoad.referenceCount = 0;
                editorAssetLoad.assetBundleName = assetBundleName;
                editorAssetLoad.loadedObjects = new Dictionary<string, Object>();

                editorAssetLoad.referenceCount++;
                return editorAssetLoad;
            }
            else
            {
                editorAssetLoad.referenceCount++;
                return editorAssetLoad;
            }
        }


        public Object Load(string assetName, System.Type type)
        {
            if (loadedObjects.TryGetValue(assetName, out Object asset) == false)
            {
                asset = AssetDatabase.LoadAssetAtPath(assetBundleName + "/" + assetName, type);

                if (asset == null)
                {
                    var assetGUIDs = AssetDatabase.FindAssets(System.IO.Path.GetFileNameWithoutExtension(assetName), new string[] { assetBundleName.Replace('_', '/') });
                    for (int i = 0; i < assetGUIDs.Length; i++)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                        if (AssetDatabase.IsValidFolder(assetPath))
                        {
                            continue;
                        }
                        asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                        break;
                    }
                    if (asset != null)
                    {
                        loadedObjects.Add(assetName, asset);
                    }
                    else
                    {
                        Debug.LogError($"资源加载失败，请检查。文件名： {assetName} 路径：{assetBundleName}");
                    }
                }
                else
                {
                    loadedObjects.Add(assetName, asset);
                }
            }
            return asset;
        }

        public void LoadAsync(UnityAction<IAssetLoader> loaded)
        {
            // todo 等待一帧以后回调      
            loaded?.Invoke(this);
        }

        public void UnloadAllLoadedObjects()
        {
            if (referenceCount == 1)
            {
                referenceCount = 0;

                loadedObjects.Clear();
            }
            else
            {
                referenceCount--;
            }
        }
    }
#endif
}