using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCardVisualComposerComponent : MonoBehaviour
{
    [Header("Combat card visual assets")]
    [Header("Card Backgrounds")]
    [SerializeField] private Sprite cardBackgroundUnknown;
    [SerializeField] private Sprite cardBackgroundInfluence;
    [SerializeField] private Sprite cardBackgroundMoney;
    [SerializeField] private Sprite cardBackgroundViolence;

    [Header("Card stats typography")]
    [SerializeField] private Sprite[] cardStatsUnknownSprites;

    private Dictionary<string, Sprite> unknownStatsNumberSpritesDictionary;

    void Awake()
    {
        InitUnknownStatsNumberSpritesDictionary();
    }

    void InitUnknownStatsNumberSpritesDictionary()
    {
        unknownStatsNumberSpritesDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < cardStatsUnknownSprites.Length; i++)
        {
            unknownStatsNumberSpritesDictionary.Add(i.ToString(), cardStatsUnknownSprites[i]);
        }
    }

    public Sprite GetCardBackgroundSprite(CombatTypes combatType)
    {
        Sprite cardBackgroundSprite;

        switch (combatType)
        {
            case CombatTypes.Influence:
                cardBackgroundSprite = cardBackgroundInfluence;
                break;
            case CombatTypes.Money:
                cardBackgroundSprite = cardBackgroundMoney;
                break;
            case CombatTypes.Violence:
                cardBackgroundSprite = cardBackgroundViolence;
                break;
            default:
                cardBackgroundSprite = cardBackgroundUnknown;
                break;
        }

        return cardBackgroundSprite;
    }

    public Sprite GetCardBackgroundUnknownSprite()
    {
        return cardBackgroundUnknown;
    }

    public Sprite GetCardCharacterUnknownSprite(CombatCardTemplate combatCardTemplate)
    {
        return combatCardTemplate.CharacterSpriteUnknown;
    }

    public (Sprite, Sprite) GetUnknownStatsNumberAsSprites(int stat)
    {
        return unknownStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
    }
}
