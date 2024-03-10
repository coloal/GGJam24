using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BrainTagOptionPicker))]
public class BrainTagOptionPickerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect popup1Rect = new Rect(position.x, position.y, position.width, position.height / 2f);
        Rect popup2Rect = new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f);

        int FirstOption = property.FindPropertyRelative("selectedTag").intValue;
        property.FindPropertyRelative("selectedTag").intValue = EditorGUI.Popup(popup1Rect, FirstOption, StateInfo.info.Select(x=>(x.Item1)).ToArray());

        FirstOption = property.FindPropertyRelative("selectedTag").intValue;
        int option = property.FindPropertyRelative("selectedTagState").intValue;
        position.y += EditorGUIUtility.singleLineHeight;
        property.FindPropertyRelative("selectedTagState").intValue = EditorGUI.Popup(popup2Rect, option, StateInfo.info[FirstOption].Item2.ToArray());
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label)*2;
    }

}
