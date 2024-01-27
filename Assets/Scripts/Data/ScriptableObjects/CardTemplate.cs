using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]


[System.Serializable]
public class HitmanInfo
{
    public int ViolenceStat = 0;
    public int MoneyStat = 0;
    public int InfluenceStat = 0;
}

public class CardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Background;

    //Incrementos/Decrementos
    public int ViolenceStat; //Nombre provisional pdt
    public int MoneyStat; //Nombre provisional pdt
    public int InfluenceStat; //Nombre provisional pdt

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CardSprite;

    public List<HitManTypes> ListHitmanTypes;
    public HitmanInfo Contable;
    public HitmanInfo Maton;
    public HitmanInfo Comisario;
}
