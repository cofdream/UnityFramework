using UnityEditor;
using UnityEditor.UI;
using Cofdream.UI;

namespace Cofdream.UnityEditor.UI
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