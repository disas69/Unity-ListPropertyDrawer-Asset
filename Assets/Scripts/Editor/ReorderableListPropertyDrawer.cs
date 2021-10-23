using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;
using System;

namespace Framework.Editor
{
    public static class ReorderableListPropertyDrawer
    {
        private static readonly Dictionary<SerializedProperty, ReorderableList> _lists = new Dictionary<SerializedProperty, ReorderableList>();

        public static void Draw(SerializedProperty serializedProperty)
        {
            Draw(serializedProperty, DefaultDrawElement);
        }

        public static void Draw(SerializedProperty serializedProperty, Action<SerializedProperty, Rect, int, bool, bool> elementDrawCallback)
        {
            var list = GetList(serializedProperty);
            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, $"{serializedProperty.displayName} [{list.count}]");
            };
            list.drawElementCallback = (rect, index, active, focused) =>
            {
                elementDrawCallback(serializedProperty, rect, index, active, focused);
            };
            list.elementHeightCallback = index => GetListHeight(serializedProperty, index);
            list.DoLayoutList();
        }

        public static void Dispose(SerializedProperty serializedProperty)
        {
            _lists.Remove(serializedProperty);
        }

        private static ReorderableList GetList(SerializedProperty serializedProperty)
        {
            if (!_lists.TryGetValue(serializedProperty, out var list))
            {
                list = new ReorderableList(serializedProperty.serializedObject, serializedProperty);
                _lists.Add(serializedProperty, list);
            }

            return list;
        }

        private static float GetListHeight(SerializedProperty serializedProperty, int index)
        {
            return EditorGUI.GetPropertyHeight(serializedProperty.GetArrayElementAtIndex(index));
        }

        private static void DefaultDrawElement(SerializedProperty property, Rect rect, int index, bool isActive, bool isFocused)
        {
            var guiContent = new GUIContent($"Element {index}");
            var element = property.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element, guiContent, true);
        }
    }
}