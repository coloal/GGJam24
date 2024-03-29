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

    [Header("Card stats sprites")]
    [Header("Unknown type sprites")]
    [SerializeField] private Sprite[] cardStatsNumbersUnknownSprites;
    [SerializeField] private Sprite[] cardHpStatsNumbersUnknownSprites;
    [SerializeField] private Sprite cardHpSeparatorUnknownSprite;

    [Header("Influence type sprites")]
    [SerializeField] private Sprite[] cardEnergyStatsInfluenceSprites;
    [SerializeField] private Sprite[] cardStatsNumbersInfluenceSprites;
    [SerializeField] private Sprite[] cardHpStatsNumbersInfluenceSprites;
    [SerializeField] private Sprite cardHpSeparatorInfluenceSprite;

    [Header("Money type sprites")]
    [SerializeField] private Sprite[] cardEnergyStatsMoneySprites;
    [SerializeField] private Sprite[] cardStatsNumbersMoneySprites;
    [SerializeField] private Sprite[] cardHpStatsNumbersMoneySprites;
    [SerializeField] private Sprite cardHpSeparatorMoneySprite;


    [Header("Violence type sprites")]
    [SerializeField] private Sprite[] cardEnergyStatsViolenceSprites;
    [SerializeField] private Sprite[] cardStatsNumbersViolenceSprites;
    [SerializeField] private Sprite[] cardHpStatsNumbersViolenceSprites;
    [SerializeField] private Sprite cardHpSeparatorViolenceSprite;

    private Dictionary<string, Sprite> unknownStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> unknownHpStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> influenceStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> influenceHpStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> moneyStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> moneyHpStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> violenceStatsNumberSpritesDictionary;
    private Dictionary<string, Sprite> violenceHpStatsNumberSpritesDictionary;

    void Awake()
    {
        InitUnknownStatsSpritesDictionary();
        InitInfluenceStatsSpritesDictionary();
        InitMoneyStatsSpritesDictionary();
        InitViolenceStatsSpritesDictionary();
    }

    void InitUnknownStatsSpritesDictionary()
    {
        SpritesUtils.InitNumberSpritesDictionary(out unknownStatsNumberSpritesDictionary, cardStatsNumbersUnknownSprites);
        SpritesUtils.InitNumberSpritesDictionary(out unknownHpStatsNumberSpritesDictionary, cardHpStatsNumbersUnknownSprites);
    }

    void InitInfluenceStatsSpritesDictionary()
    {
        SpritesUtils.InitNumberSpritesDictionary(out influenceStatsNumberSpritesDictionary, cardStatsNumbersInfluenceSprites);
        SpritesUtils.InitNumberSpritesDictionary(out influenceHpStatsNumberSpritesDictionary, cardHpStatsNumbersInfluenceSprites);
    }

    void InitMoneyStatsSpritesDictionary()
    {
        SpritesUtils.InitNumberSpritesDictionary(out moneyStatsNumberSpritesDictionary, cardStatsNumbersMoneySprites);
        SpritesUtils.InitNumberSpritesDictionary(out moneyHpStatsNumberSpritesDictionary, cardHpStatsNumbersMoneySprites);
    }

    void InitViolenceStatsSpritesDictionary()
    {
        SpritesUtils.InitNumberSpritesDictionary(out violenceStatsNumberSpritesDictionary, cardStatsNumbersViolenceSprites);
        SpritesUtils.InitNumberSpritesDictionary(out violenceHpStatsNumberSpritesDictionary, cardHpStatsNumbersViolenceSprites);
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

    public Sprite GetCardCharacterSprite(CombatCardTemplate combatCardTemplate)
    {
        Sprite cardCharacterSprite;

        switch (combatCardTemplate.CombatType)
        {
            case CombatTypes.Influence:
                cardCharacterSprite = combatCardTemplate.CharacterSpriteInlfuence;
                break;
            case CombatTypes.Money:
                cardCharacterSprite = combatCardTemplate.CharacterSpriteMoney;
                break;
            case CombatTypes.Violence:
                cardCharacterSprite = combatCardTemplate.CharacterSpriteViolence;
                break;
            default:
                cardCharacterSprite = combatCardTemplate.CharacterSpriteInlfuence;
                break;
        }

        return cardCharacterSprite;
    }

    public Sprite GetCardCharacterUnknownSprite(CombatCardTemplate combatCardTemplate)
    {
        return combatCardTemplate.CharacterSpriteUnknown;
    }

    public (Sprite, Sprite) GetUnknownStatsNumberAsSprites(int stat)
    {
        return unknownStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
    }

    public (Sprite, Sprite) GetUnknownHpStatsNumberAsSprites(int stat)
    {
        return unknownHpStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
    }

    public Sprite GetEnergyCellStatSprite(CombatTypes combatType, int numberOfEnergyCells, int energyCell)
    {
        Sprite[] energyStatsSprites = cardEnergyStatsInfluenceSprites;
        
        switch (combatType)
        {
            case CombatTypes.Influence:
                energyStatsSprites = cardEnergyStatsInfluenceSprites;
                break;
            case CombatTypes.Money:
                energyStatsSprites = cardEnergyStatsMoneySprites;
                break;
            case CombatTypes.Violence:
                energyStatsSprites = cardEnergyStatsViolenceSprites;
                break;
        }

        // CAREFUL! We start counting energy cells from 0
        return energyStatsSprites[energyCell / numberOfEnergyCells];
    }

    public (Sprite, Sprite) GetStatsNumberAsSprites(CombatTypes combatType, int stat)
    {
        switch (combatType)
        {
            case CombatTypes.Influence:
                return influenceStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
            case CombatTypes.Money:
                return moneyStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
            case CombatTypes.Violence:
                return violenceStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
        }

        return unknownStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
    }

    public (Sprite, Sprite) GetHpStatsNumberAsSprites(CombatTypes combatType, int stat)
    {
        switch (combatType)
        {
            case CombatTypes.Influence:
                return influenceHpStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
            case CombatTypes.Money:
                return moneyHpStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
            case CombatTypes.Violence:
                return violenceHpStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
        }

        return unknownHpStatsNumberSpritesDictionary.GetNumbersAsSprites(stat);
    }

    public Sprite GetCardHpSeparatorUnknownSprite()
    {
        return cardHpSeparatorUnknownSprite;
    }

    public Sprite GetCardHpSeparatorSprite(CombatTypes combatType)
    {
        switch (combatType)
        {
            case CombatTypes.Influence:
                return cardHpSeparatorInfluenceSprite;
            case CombatTypes.Money:
                return cardHpSeparatorMoneySprite;
            case CombatTypes.Violence:
                return cardHpSeparatorViolenceSprite;
        }

        return cardHpSeparatorUnknownSprite;
    }
}
