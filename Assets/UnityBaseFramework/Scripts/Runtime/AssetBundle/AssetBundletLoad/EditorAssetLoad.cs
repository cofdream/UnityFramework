using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.AssetLoad
{
#if UNITY_EDITOR
    public class EditorAssetLoad : IAssetLoad
    {
        public static bool LocalLoadModel;

        public const bool DefaultValue = true;
        public const string LOCAL_LOAD_MODEL = "EditorAssetLoad.localLoadModel";
        private const string MENI_ITEM_NAME = "Switch Editor Asset Load Model/LocalLoadModel";

        static EditorAssetLoad()
        {
            LocalLoadModel = EditorPrefs.GetBool(LOCAL_LOAD_MODEL, DefaultValue);
        }


        [MenuItem(MENI_ITEM_NAME, true)]
        public static bool InitLocalLoadModel()
        {
            if (EditorApplication.isPlaying) return false;

            Menu.SetChecked(MENI_ITEM_NAME, LocalLoadModel);
            return true;
        }

        [MenuItem(MENI_ITEM_NAME)]
        public static void SwitchLocalLoadModel()
        {
            LocalLoadModel = !LocalLoadModel;
            EditorPrefs.SetBool(LOCAL_LOAD_MODEL, LocalLoadModel);

            Menu.SetChecked(MENI_ITEM_NAME, LocalLoadModel);
        }

        [MenuItem("1/TT")]
        public static void Temp()
        {
            GUIUtility.systemCopyBuffer = GUIUtility.systemCopyBuffer.Replace('/', '_');

            Resources.UnloadUnusedAssets();
            AssetBundle.UnloadAllAssetBundles(true);
        }


        private uint referenceCount;
        private string assetBundleName;
        private string[] assetRootPath;

        private Dictionary<string, Object> allAsset;

        private static Dictionary<string, EditorAssetLoad> allEditorAssetLoad = new Dictionary<string, EditorAssetLoad>();

        EditorAssetLoad() { }

        public static EditorAssetLoad Take(string assetBundleName)
        {
            EditorAssetLoad editorAssetLoad;
            if (allEditorAssetLoad.TryGetValue(assetBundleName, out editorAssetLoad) == false)
            {
                //todo 验证是ab包名正确性 

                editorAssetLoad = new EditorAssetLoad();
                allEditorAssetLoad.Add(assetBundleName, editorAssetLoad);

                editorAssetLoad.referenceCount = 0;
                editorAssetLoad.assetBundleName = assetBundleName;
                editorAssetLoad.assetRootPath = new string[] { assetBundleName.Replace('_', '/') };
                editorAssetLoad.allAsset = new Dictionary<string, Object>();

                editorAssetLoad.referenceCount++;
                return editorAssetLoad;
            }
            else
            {
                editorAssetLoad.referenceCount++;
                return editorAssetLoad;
            }
        }
        public static void Put(EditorAssetLoad editorAssetLoad)
        {
            if (editorAssetLoad.referenceCount == 1)
            {
                editorAssetLoad.referenceCount = 0;

                editorAssetLoad.allAsset.Clear();
            }
            else
            {
                editorAssetLoad.referenceCount--;
            }
        }

        public Object LoadAsset(string assetName, System.Type type)
        {
            Object asset;
            if (allAsset.TryGetValue(assetName, out asset) == false)
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

                Debug.Log(asset);

                allAsset.Add(assetName, asset);
            }
            return asset;
        }

        public void UnAllLoad()
        {
            Put(this);
        }
    }
#endif
}