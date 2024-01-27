using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")]
public class CardTemplate : ScriptableObject
{
    //Information of the card
    public string NameOfCard;
    public string Name;
    public string Gen;
    public int Age;
    public string Background;

    //Indice del chat a reproducir se debe autoincrementar al terminar con esta carta
    public int indexChats = 0;
    public List <string> Chats;
    
    public List<string> LeftChoices;
    public List<string> RightChoices;

    //next cards to appear
    public List<GameObject> LeftCards;
    public List<GameObject> RightCards;

    //Incrementos/Decrementos
    public int ViolenceStat; //Nombre provisional pdt
    public int MoneyStat; //Nombre provisional pdt
    public int InfluenceStat; //Nombre provisional pdt

    //Choices text
    public string LeftText;
    public string RightText;

    //Card sprite
    public Sprite CardSprite;
}
