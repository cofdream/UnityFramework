using Cofdream.UI.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.UI
{
    public static class UIManager
    {
        public static UILayers UILayers;

     
        public static void Init()
        {
            Debug.Log(UILayers);
        }

        public static GameObject OpenPanel(GameObject panel)
        {
            var obj = Object.Instantiate(panel, UILayers.Default);
            //obj.transform.localPosition = Vector3.zero;

            return obj;
        }

        public static void ClosePanel(GameObject mainPanel)
        {
            Object.Destroy(mainPanel);
        }
    }
}
