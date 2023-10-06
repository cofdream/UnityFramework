using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.UI.Core
{
    [DisallowMultipleComponent]
    public class UILayers : MonoBehaviour
    {
        public RectTransform Default;

        public void Awake()
        {
            UIManager.UILayers = this;    
        }
    }
}
