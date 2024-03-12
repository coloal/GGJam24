using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]


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
    public NumericTags NumericTag;
    public int Increment;
    public BrainNumericTagAction BrainNumericTagAction;

    //State
    public string TagState;
    public string NewState;
    public BrainStateTagAction BrainStateTagAction;

}



[System.Serializable]
public class CombatInfo
{
    public int HealthPoints = 0;
    public int Damage = 0;
    public int Armor = 0;
    public CombatTypes CombatType;
}

public class CardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Background;

    /*// --Deprecated--
    //Incrementos/Decrementos
    public int ViolenceStat; //Nombre provisional pdt
    public int MoneyStat; //Nombre provisional pdt
    public int InfluenceStat; //Nombre provisional pdt
    */

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CardSprite;
    public Sprite BackgroundSprite;

    public CombatInfo CombatInfo;

    public List<Option> LeftActions;
    public List<Option> RightActions;


    /*// --Deprecated--
    public List<HitManTypes> ListHitmanTypes;
    public HitmanInfo Contable;
    public HitmanInfo Maton;
    public HitmanInfo Comisario;
    */
}
