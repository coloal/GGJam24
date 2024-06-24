using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatCard", menuName = "Combat Card")]
public class CombatCardTemplate : ScriptableObject
{
    public string NameOfCard;

    //Card sprite
    public Sprite cardSprite;

    //Combat card stats
    public CombatTypes CombatType;

    //Instrument associated
    public string Instrument;
}