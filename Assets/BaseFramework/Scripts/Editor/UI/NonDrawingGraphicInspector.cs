using UnityEditor;
using UnityEditor.UI;
using Cofdream.BaseFramework.UnityEngine.UI;

namespace Cofdream.BaseFramework.UnityEditor.UI
{
    [CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
    internal sealed class NonDrawingGraphicInspector : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Script);

            RaycastControlsGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}