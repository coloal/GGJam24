using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumPicker))]
public class EnumPickerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        

        Rect popup1Rect = new Rect(position.x, position.y, position.width, position.height / 2f);
        Rect popup2Rect = new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f);

        int FirstOption = property.FindPropertyRelative("selectedFirstOption").intValue;
        property.FindPropertyRelative("selectedFirstOption").intValue = EditorGUI.Popup(popup1Rect, FirstOption, System.Enum.GetNames(typeof(EnumTag)));
        FirstOption = property.FindPropertyRelative("selectedFirstOption").intValue;
        int option = property.FindPropertyRelative("selectedSecondOption").intValue;
        position.y += EditorGUIUtility.singleLineHeight;
        if (((int)EnumTag.MasterSword) == FirstOption)
        {
            property.FindPropertyRelative("selectedSecondOption").intValue = EditorGUI.Popup(popup2Rect, option, System.Enum.GetNames(typeof(MasterSword)));
        }
        else if (((int)EnumTag.John) == FirstOption)
        {
            property.FindPropertyRelative("selectedSecondOption").intValue = EditorGUI.Popup(popup2Rect, option, System.Enum.GetNames(typeof(John)));
        }
        
        
        

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label)*2;
    }

}
