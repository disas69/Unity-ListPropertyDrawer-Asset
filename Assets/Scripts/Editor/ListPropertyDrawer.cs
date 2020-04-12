using System;
using UnityEditor;
using UnityEngine;
using Framework.Editor.GUIUtilities;

namespace Framework.Editor
{
    public static class ListPropertyDrawer
    {
        public static void OnGUI(SerializedProperty property)
        {
            OnGUI(property, DrawElement);
        }

        public static void OnGUI(SerializedProperty property, Action<SerializedProperty, int> elementDrawCallback)
        {
            if (!property.isArray)
            {
                throw new ArgumentException("ListPropertyDrawer supports only array properties");
            }

            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, new GUIContent($"{property.displayName} ({property.arraySize})"));
                    
                    if (GUILayout.Button(new GUIContent("+", "Add new element"), GUILayout.Width(40f)))
                    {
                        property.InsertArrayElementAtIndex(property.arraySize);

                        if (!property.isExpanded)
                        {
                            property.isExpanded = true;
                        }

                        property.GetArrayElementAtIndex(property.arraySize - 1).isExpanded = true;
                    }
                }

                if (property.isExpanded)
                {
                    using (new GUIIndent())
                    {
                        var count = property.arraySize;
                        if (count > 0)
                        {
                            var indexToRemove = -1;
                            for (var i = 0; i < count; i++)
                            {
                                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                                {
                                    using (new EditorGUILayout.VerticalScope())
                                    {
                                        elementDrawCallback?.Invoke(property.GetArrayElementAtIndex(i), i);
                                    }

                                    using (new GUIEnabled(i > 0))
                                    {
                                        if (GUILayout.Button(new GUIContent("<", "Move element back"), GUILayout.Width(25f)))
                                        {
                                            property.MoveArrayElement(i, i - 1);
                                        }
                                    }

                                    using (new GUIEnabled(i < count - 1))
                                    {
                                        if (GUILayout.Button(new GUIContent(">", "Move element forward"), GUILayout.Width(25f)))
                                        {
                                            property.MoveArrayElement(i, i + 1);
                                        }
                                    }

                                    if (GUILayout.Button(new GUIContent("×", "Remove element"), GUILayout.Width(25f)))
                                    {
                                        indexToRemove = i;
                                    }
                                }
                            }

                            if (indexToRemove >= 0)
                            {
                                property.DeleteArrayElementAtIndex(indexToRemove);
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Empty...");
                        }
                    }
                }
            }
        }

        private static void DrawElement(SerializedProperty element, int index)
        {
            if (element.propertyType == SerializedPropertyType.Generic)
            {
                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, $"Element [{index}]");

                if (element.isExpanded)
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new GUIIndent())
                        {
                            foreach (var property in element.GetChildren())
                            {
                                EditorGUILayout.PropertyField(property, true);
                            }
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.PropertyField(element, new GUIContent($"{element.propertyType} [{index}]"), true);
            }
        }
    }
}