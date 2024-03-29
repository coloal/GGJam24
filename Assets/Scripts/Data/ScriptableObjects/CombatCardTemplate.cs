using UnityEngine;

[CreateAssetMenu(fileName = "New CombatCardData", menuName = "CombatCardData")]
public class CombatCardTemplate : ScriptableObject
{
    public string NameOfCard;

    //Dialog Text
    public string InitialText;
    public string EffectiveText;
    public string NonEffectiveText;

    //Card sprite
    public Sprite CharacterSpriteUnknown;
    public Sprite CharacterSpriteInlfuence;
    public Sprite CharacterSpriteMoney;
    public Sprite CharacterSpriteViolence;

    //Combat card stats
    public int HealthPoints = 0;
    public int Damage = 0;
    public int Armor = 0;
    public int Turns = 0;
    public CombatTypes CombatType;

    //Instrument associated
    public string Instrument;
}