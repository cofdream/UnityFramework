using Cofdream.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIRoot : MonoBehaviour
{
#if UNITY_EDITOR

    private static EditorBuildSettingsScene[] oldScenes;


    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private static void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingEditMode)
        {
            // 把对应场景添加到 EditorBuildSettings 中，并设置是否激活该场景Scene
            var sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
            var settingsScenes = new EditorBuildSettingsScene[sceneGUIDs.Length];
            for (int i = 0; i < sceneGUIDs.Length; i++)
            {
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]);
                settingsScenes[i] = new EditorBuildSettingsScene(scenePath, true);
            }

            oldScenes = EditorBuildSettings.scenes;
            EditorBuildSettings.scenes = settingsScenes;
        }
        else if (change == PlayModeStateChange.EnteredEditMode)
        {
            EditorBuildSettings.scenes = null;
        }
    }

    private static void StaticStart()
    {
        //SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        var asyncOperation = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        asyncOperation.completed += OnLoadScene;
    }

    private static void OnLoadScene(AsyncOperation operation)
    {
        if (operation.isDone == false)
            return;
    }

#endif


    public GameObject MainPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {

    }

    private void Start()
    {
        StaticStart();

        UIManager.Init();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            panels.Add(UIManager.OpenPanel(MainPanel));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var panel in panels)
            {
                UIManager.ClosePanel(panel);
            }
            panels.Clear();
        }
    }

}
