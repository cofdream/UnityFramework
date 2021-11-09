using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

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
        private string[] assetRootPath;

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
                editorAssetLoad.assetRootPath = new string[] { assetBundleName.Replace('_', '/') };
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
                var assetGUIDs = AssetDatabase.FindAssets(System.IO.Path.GetFileNameWithoutExtension(assetName), assetRootPath);

                for (int i = 0; i < assetGUIDs.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                    if (AssetDatabase.IsValidFolder(path))
                    {
                        continue;
                    }
                    asset = AssetDatabase.LoadAssetAtPath(path, type);
                    break;
                }
                loadedObjects.Add(assetName, asset);
            }
            return asset;
        }
        public void LoadAsync() { }
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