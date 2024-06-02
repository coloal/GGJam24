using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CombatCardData", menuName = "CombatCardData")]
public class CombatCardTemplate : ScriptableObject
{
    public string NameOfCard;

    //Card sprite
    public Sprite CharacterSprite;

    //Combat card stats
    public CombatTypes CombatType;

    //Instrument associated
    public string Instrument;
}