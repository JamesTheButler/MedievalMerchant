using Common.Config.Sampling;
using UnityEditor;
using UnityEngine;

namespace Editor.Sampling
{
    /// <summary>
    /// Shows each keyframe's approximate share of the sampling weight live, so a designer can
    /// balance a curve-based distribution without having to sample it first.
    /// </summary>
    [CustomPropertyDrawer(typeof(NonUniformSampler))]
    public sealed class NonUniformSamplerDrawer : PropertyDrawer
    {
        private const int IntegrationSteps = 100;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var curveProperty = property.FindPropertyRelative("<Curve>k__BackingField");
            var minimumProperty = property.FindPropertyRelative("<Minimum>k__BackingField");
            var maximumProperty = property.FindPropertyRelative("<Maximum>k__BackingField");
            var resolutionProperty = property.FindPropertyRelative("<Resolution>k__BackingField");

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;
            var y = position.y;

            var foldoutRect = new Rect(position.x, y, position.width, lineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            y += lineHeight + spacing;

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            DrawField(ref y, position, curveProperty, lineHeight, spacing);
            DrawField(ref y, position, minimumProperty, lineHeight, spacing);
            DrawField(ref y, position, maximumProperty, lineHeight, spacing);
            DrawField(ref y, position, resolutionProperty, lineHeight, spacing);

            DrawKeyframeWeights(ref y, position, curveProperty, minimumProperty, maximumProperty, lineHeight, spacing);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        private static void DrawKeyframeWeights(
            ref float y,
            Rect position,
            SerializedProperty curveProperty,
            SerializedProperty minimumProperty,
            SerializedProperty maximumProperty,
            float lineHeight,
            float spacing)
        {
            var curve = curveProperty.animationCurveValue;
            if (curve == null || curve.length == 0)
                return;

            var peak = 0f;
            foreach (var key in curve.keys)
                peak = Mathf.Max(peak, key.value);

            var minimum = minimumProperty.floatValue;
            var maximum = maximumProperty.floatValue;
            var totalWeight = 0f;
            for (var i = 0; i < IntegrationSteps; i++)
            {
                var t = i / (float)(IntegrationSteps - 1);
                var value = Mathf.Lerp(minimum, maximum, t);
                totalWeight += Mathf.Max(0f, curve.Evaluate(value));
            }

            foreach (var key in curve.keys)
            {
                var height = Mathf.Max(0f, key.value);
                var percentOfPeak = peak > 0f ? height / peak * 100f : 0f;
                var percentOfTotal = totalWeight > 0f ? height / totalWeight * 100f : 0f;

                var lineRect = new Rect(position.x, y, position.width, lineHeight);
                EditorGUI.LabelField(
                    lineRect,
                    $"x = {key.time:0.###}",
                    $"{percentOfPeak:0.#}% of peak · ~{percentOfTotal:0.#}% of total weight");
                y += lineHeight + spacing;
            }
        }

        private static void DrawField(ref float y, Rect position, SerializedProperty fieldProperty, float lineHeight, float spacing)
        {
            var rect = new Rect(position.x, y, position.width, lineHeight);
            EditorGUI.PropertyField(rect, fieldProperty);
            y += lineHeight + spacing;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var spacing = EditorGUIUtility.standardVerticalSpacing;

            if (!property.isExpanded)
                return lineHeight;

            var height = (lineHeight + spacing) * 5; // foldout, curve, min, max, resolution

            var curveProperty = property.FindPropertyRelative("<Curve>k__BackingField");
            var curve = curveProperty?.animationCurveValue;
            if (curve != null)
                height += (lineHeight + spacing) * curve.length;

            return height;
        }
    }
}
