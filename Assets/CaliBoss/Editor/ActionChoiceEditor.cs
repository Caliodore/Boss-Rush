using System;
using UnityEditor;
using UnityEngine;

namespace Cali7
{
    public class ActionChoiceEditor
    {
        public class ActionChoiceDrawerAttribute : PropertyAttribute { 
            public ActionChoiceDrawerAttribute() : base(false) { }
        }

        // IngredientDrawer
        [CustomPropertyDrawer(typeof(ActionChoiceDrawerAttribute))]
        public class ActionChoiceDrawer : PropertyDrawer
        {
            // Draw the property inside the given rect
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                // Using BeginProperty / EndProperty on the parent property means that
                // prefab override logic works on the entire property.
                EditorGUI.BeginProperty(position, label, property);

                // Draw label
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

                // Don't make child fields be indented
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                // Calculate rects
                var nameRect = new Rect(position.x, position.y, 30, position.height);
                var meleeRect = new Rect(position.x + 35, position.y, 50, position.height);
                var readyRect = new Rect(position.x + 50, position.y, position.width - 50, position.height);
                var timeRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

                // Draw fields - pass GUIContent.none to each so they are drawn without labels
                EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("actionName"), GUIContent.none);
                EditorGUI.PropertyField(readyRect, property.FindPropertyRelative("isReady"), GUIContent.none);
                EditorGUI.PropertyField(meleeRect, property.FindPropertyRelative("isMelee"), GUIContent.none);
                EditorGUI.PropertyField(timeRect, property.FindPropertyRelative("cooldownTime"), GUIContent.none);

                // Set indent back to what it was
                EditorGUI.indentLevel = indent;

                EditorGUI.EndProperty();
            }
        }

    }
}
