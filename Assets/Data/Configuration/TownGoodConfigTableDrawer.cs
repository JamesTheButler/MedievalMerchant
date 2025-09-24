#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data.Configuration
{
    [CustomPropertyDrawer(typeof(TownGoodConfigTable<>), true)]
    public sealed class TownGoodConfigTableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label, true);

            if (!property.isExpanded)
                return;

            EditorGUI.indentLevel++;

            // Draw children in order
            var town1Good1 = property.FindPropertyRelative("town1Good1");
            var town2Good1 = property.FindPropertyRelative("town2Good1");
            var town2Good2 = property.FindPropertyRelative("town2Good2");
            var town3Good1 = property.FindPropertyRelative("town3Good1");
            var town3Good2 = property.FindPropertyRelative("town3Good2");
            var town3Good3 = property.FindPropertyRelative("town3Good3");

            DrawTownBox("Tier 1 Town", town1Good1);
            DrawTownBox("Tier 2 Town", town2Good1, town2Good2);
            DrawTownBox("Tier 3 Town", town3Good1, town3Good2, town3Good3);

            EditorGUI.indentLevel--;
        }

        private void DrawTownBox(string title, params SerializedProperty[] props)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            foreach (var p in props)
            {
                EditorGUILayout.PropertyField(p, new GUIContent($"Good Tier {p.name.Last()}"));
            }

            EditorGUILayout.EndVertical();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight; // we use GUILayout so height doesn’t matter much
        }
    }
}
#endif