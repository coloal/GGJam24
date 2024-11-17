using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BrainAction
{
    [Header("Selecciona tipo de tag a modificar")]
    public BrainTagType TagType;

    //Bool
    [Space]
    [Header("Booleana")]
    public BrainTag BoolTag;
    public bool NewValue;

    //Numeric 
    [Space]
    [Header("Numerica")]
    public NumericTags NumericTag;
    public int Increment;

    //State
    [Space]
    [Header("Estado")]
    public string TagState;
    public string NewState;
}

[System.Serializable]
public class SoundAction
{
    public string SoundTag;
    public float NewValue;
}

[System.Serializable]
public class SoundTypeCard
{
    public SoundAction MoneySoundAction;

    public SoundAction ViolenceSoundAction;

    public SoundAction InfluenceSoundAction;
}

/*[CustomPropertyDrawer(typeof(BrainAction))]
public class Option_Editor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        BrainAction opt = (BrainAction)property.boxedValue;
        EditorGUI.BeginProperty(position, label, property);
        Rect Rect1 = new Rect(position.x, position.y, position.width, position.height / 5f);
        Rect Rect2 = new Rect(position.x, Rect1.y + Rect1.height, position.width, position.height / 5f);
        Rect Rect3 = new Rect(position.x, Rect2.y + Rect2.height, position.width, position.height / 5f);
        Rect Rect4 = new Rect(position.x, Rect3.y + Rect3.height, position.width, position.height / 5f);
        Rect Rect5 = new Rect(position.x, Rect4.y + Rect4.height, position.width, position.height / 5f);


        EditorGUI.LabelField(Rect1, "Type of Action");
        BrainTagType a = (BrainTagType)EditorGUI.EnumPopup(Rect2, opt.TagType);
        property.FindPropertyRelative("TagType").intValue = (int)a;

        switch ((opt.TagType))
        {
            case BrainTagType.Bool:
                EditorGUI.LabelField(Rect3, "Parameters");
                BrainTag b = (BrainTag)EditorGUI.EnumPopup(Rect4, opt.BoolTag);
                property.FindPropertyRelative("BoolTag").intValue = (int)b;
                property.FindPropertyRelative("NewValue").boolValue = EditorGUI.Toggle(Rect5, opt.NewValue);
                break;

            case BrainTagType.Numeric:
                EditorGUI.LabelField(Rect3, "Parameters");
                NumericTags c = (NumericTags)EditorGUI.EnumPopup(Rect4, opt.NumericTag);
                property.FindPropertyRelative("NumericTag").intValue = (int)c;
                property.FindPropertyRelative("Increment").intValue = EditorGUI.IntField(Rect5, opt.Increment);
                break;

            case BrainTagType.State:
                EditorGUI.LabelField(Rect3, "Parameters");
                property.FindPropertyRelative("TagState").intValue = EditorGUI.Popup(Rect4, opt.TagState, StateInfo.info.Select(x => (x.Item1)).ToArray());
                property.FindPropertyRelative("NewState").intValue = EditorGUI.Popup(Rect5, opt.NewState, StateInfo.info[opt.TagState].Item2.ToArray());
                break;
        }
        EditorGUI.EndProperty();



    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 6;
    }
}*/