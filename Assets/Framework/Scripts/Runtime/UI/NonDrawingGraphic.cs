using UnityEngine;
using UnityEngine.UI;

namespace Cofdream.UnityBaseFramework.UI
{

    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class NonDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
        }
    }
}