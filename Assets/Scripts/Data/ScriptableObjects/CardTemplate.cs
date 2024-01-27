using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
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
}
