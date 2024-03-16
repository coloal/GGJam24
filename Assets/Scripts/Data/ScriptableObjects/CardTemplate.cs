using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;


[System.Serializable]
public class HitmanInfo
{
    public string Name;
    public string FeedbackName;
    public string FeedbackText;
    public int ViolenceStat = 0;
    public int MoneyStat = 0;
    public int InfluenceStat = 0;
}

[System.Serializable]
public class Option 
{
    public BrainTagType TagType;
    //Bool
    public BrainTag BoolTag;
    public bool NewValue;
    public BrainBoolTagAction BrainBoolTagAction;

    //Numeric 
    [HideInInspector] public NumericTags NumericTag;
    [HideInInspector] public int Increment;
    [HideInInspector] public BrainNumericTagAction BrainNumericTagAction;

    //State
    [HideInInspector] public BrainTagOptionPicker StateTuple;
    
    public int TagState;
    public int NewState;
    
    public BrainStateTagAction BrainStateTagAction;
    [HideInInspector] public BrainStateIntTagAction BrainStateIntTagAction;
}

/*public void Init()
{
    if (BrainBoolTagAction.GetPersistentEventCount() == 0)
    {
        BrainBoolTagAction.SetPersistentListenerState(0, BrainActions.SetBrainBoolTag);
        //UnityEventTools.AddPersistentListener(BrainBoolTagAction, BrainActions.SetBrainBoolTag);
    }


    UnityEventTools.AddPersistentListener(BrainNumericTagAction, BrainActions.SetBrainNumericTag);

}

void Awake()
{
    UnityEventTools.AddPersistentListener(BrainBoolTagAction, BrainActions.SetBrainBoolTag);
}*/


[System.Serializable]
public class CombatInfo
{
    public int HealthPoints = 0;
    public int Damage = 0;
    public int Armor = 0;
    public int Turns = 0;
    //public int Energy = 0;
    public CombatTypes CombatType;

    public string InitialText;
    public string EffectiveText;
    public string NonEffectiveText;
    public Sprite BackgroundCombatSprite;
}

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
public class CardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Background;

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CardSprite;
    public Sprite BackgroundSprite;

    public CombatInfo CombatInfo;

    public List<Option> LeftActions = new List<Option>();
    public List<Option> RightActions = new List<Option>();
}


[CustomPropertyDrawer(typeof(Option))]
public class Option_Editor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Option opt = (Option)property.boxedValue;
        EditorGUI.BeginProperty(position, label, property);
        Rect Rect1 = new Rect(position.x, position.y, position.width, position.height/5f);
        Rect Rect2 = new Rect(position.x, Rect1.y + Rect1.height, position.width, position.height/5f);
        Rect Rect3 = new Rect(position.x, Rect2.y + Rect2.height, position.width, position.height / 5f);
        Rect Rect4 = new Rect(position.x, Rect3.y + Rect3.height, position.width, position.height / 5f);
        Rect Rect5 = new Rect(position.x, Rect4.y + Rect4.height, position.width, position.height / 5f);


        EditorGUI.LabelField(Rect1,"Type of Action");
        BrainTagType a =(BrainTagType) EditorGUI.EnumPopup(Rect2, opt.TagType);
        property.FindPropertyRelative("TagType").intValue = (int)a;

        switch ((opt.TagType))
        {
            case BrainTagType.Bool:
                EditorGUI.LabelField(Rect3,"Parameters");
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
    /*
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        Option opt = (Option)property.boxedValue;
        VisualElement container = new VisualElement();

        var TagType = new PropertyField(property.FindPropertyRelative("TagType"));
        container.Add(TagType);

        switch ((opt.TagType))
        {
            case BrainTagType.Bool:
                var BoolTag = new PropertyField(property.FindPropertyRelative("BoolTag"));
                var NewValue = new PropertyField(property.FindPropertyRelative("NewValue"));
                var BrainBoolTagAction = new PropertyField(property.FindPropertyRelative("BrainBoolTagAction"));

                container.Add(BoolTag);
                container.Add(NewValue);
                container.Add(BrainBoolTagAction);
                break;
            case BrainTagType.Numeric:
                var NumericTag = new PropertyField(property.FindPropertyRelative("NumericTag"));
                var Increment = new PropertyField(property.FindPropertyRelative("Increment"));
                var BrainNumericTagAction = new PropertyField(property.FindPropertyRelative("BrainNumericTagAction"));
                container.Add(NumericTag);
                container.Add(Increment);
                container.Add(BrainNumericTagAction);
                break;
            case BrainTagType.State:
                var StateTuple = new PropertyField(property.FindPropertyRelative("StateTuple"));
                var BrainStateIntTagAction = new PropertyField(property.FindPropertyRelative("BrainStateIntTagAction"));
                container.Add(StateTuple);
                container.Add(BrainStateIntTagAction);
                break;
        }

        return container;
    }*/
}