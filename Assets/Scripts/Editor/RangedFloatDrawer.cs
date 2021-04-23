using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RangedFloat))]
public class RangedFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        float minValue = minProp.floatValue;
        float maxValue = maxProp.floatValue;

        float rangeMin = 0;
        float rangeMax = 2;

        RangedFloatRangeAttribute[] ranges = (RangedFloatRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(RangedFloatRangeAttribute), true);
        if (ranges.Length > 0)
        {
            rangeMin = ranges[0].min;
            rangeMax = ranges[0].max;
        }

        const float rangeBoundsLabelWidth = 45f;
        
        GUIStyle centeredTextStyle = new GUIStyle("label");

        centeredTextStyle.alignment = TextAnchor.MiddleLeft;
        var rangeBoundsLabel1Rect = new Rect(position);
        rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
        GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString("F2")), centeredTextStyle);
        position.xMin += rangeBoundsLabelWidth;

        centeredTextStyle.alignment = TextAnchor.MiddleRight;
        var rangeBoundsLabel2Rect = new Rect(position);
        rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
        GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString("F2")), centeredTextStyle);
        position.xMax -= rangeBoundsLabelWidth;

        EditorGUI.BeginChangeCheck();
        EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
        if (EditorGUI.EndChangeCheck())
        {
            minProp.floatValue = minValue;
            maxProp.floatValue = maxValue;
        }

        EditorGUI.EndProperty();
    }
}