using Framework.Editor.GUIUtilities;
using System.Collections.Generic;
using UnityEditor;

namespace Framework.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class CustomInspector : UnityEditor.Editor
    {
        private List<SerializedProperty> _serializedProperties;

        private void OnEnable()
        {
            _serializedProperties = GetSerializedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            foreach (var property in _serializedProperties)
            {
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new GUIEnabled(false))
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                }
                else
                {
                    if (property.isArray)
                    {
                        ListPropertyDrawer.OnGUI(property);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private List<SerializedProperty> GetSerializedProperties()
        {
            var serializedProperties = new List<SerializedProperty>();

            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        serializedProperties.Add(serializedObject.FindProperty(iterator.name));
                    } while (iterator.NextVisible(false));
                }
            }

            return serializedProperties;
        }
    }
}