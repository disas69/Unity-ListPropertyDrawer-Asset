using UnityEditor;
using UnityEngine;

namespace Framework.Editor.GUIUtilities
{
    public class GUIIndent : GUI.Scope
    {
        private readonly int _previousIndentLevel;

        public GUIIndent() : this(EditorGUI.indentLevel + 1)
        {
        }

        public GUIIndent(int indentLevel)
        {
            _previousIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indentLevel;
        }

        protected override void CloseScope()
        {
            EditorGUI.indentLevel = _previousIndentLevel;
        }
    }
}