using UnityEngine;

namespace Framework.Editor.GUIUtilities
{
    public class GUIEnabled : GUI.Scope
    {
        private readonly bool _previousEnabled;

        public GUIEnabled(bool enabled)
        {
            _previousEnabled = GUI.enabled;
            GUI.enabled = enabled;
        }

        protected override void CloseScope()
        {
            GUI.enabled = _previousEnabled;
        }
    }
}