using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCard : Card
{
    /*
    [SerializeField] private TextMeshPro BoxNameOfCard;

    [SerializeField] private GameObject RightTextBoxContainer;
    [SerializeField] private TextMeshProUGUI RightText;

    [SerializeField] private GameObject LeftTextBoxContainer;
    [SerializeField] private TextMeshProUGUI LeftText;
    */
    [SerializeField]
    private SpriteRenderer BackgroundCombatSprite;

    [SerializeField]
    private List<SpriteRenderer> AttackPoints;

    [SerializeField]
    private List<SpriteRenderer> DefensePoints;

    [SerializeField]
    private List<SpriteRenderer> EnergyPoints;

    [SerializeField] private TextMeshPro HealthText;
    [SerializeField] private TextMeshProUGUI DialogText;

    private int HealthPoints;
    private int Damage = 0;
    private int Armor = 0;
    private int Energy = 0;
    private int Turns = 0;
    private CombatTypes CombatType;

    private string InitialText;
    private string EffectiveText;
    private string NonEffectiveText;

    override public void SetDataCard(CardTemplate DataCard)
    {
        base.SetDataCard(DataCard);

        //base.BackgroundSprite.enabled = false;
        BackgroundCombatSprite.sprite = DataCard.CombatInfo.BackgroundCombatSprite;

        HealthPoints = DataCard.CombatInfo.HealthPoints;
        Damage = DataCard.CombatInfo.Damage;
        Armor = DataCard.CombatInfo.Armor;
        Turns = DataCard.CombatInfo.Turns;
        CalculateEnergy();
        CombatType = DataCard.CombatInfo.CombatType;

        InitialText = DataCard.CombatInfo.InitialText;
        EffectiveText = DataCard.CombatInfo.EffectiveText;
        NonEffectiveText = DataCard.CombatInfo.NonEffectiveText;

        //Encendemos los puntos de cada stat
        SetStat(Damage, AttackPoints);
        SetStat(Armor, DefensePoints);
        SetStat(Energy, EnergyPoints);
        HealthText.text = HealthPoints.ToString();
        DialogText.text = InitialText;
    }

    private void SetStat(int value, List<SpriteRenderer> sprites)
    {
        for (int i = 0; i < value && i < sprites.Count; i++)
        {
            sprites[i].enabled = true;
        }
    }

    public void DecrementEnergy()
    {
        Energy -= 1;
        EnergyPoints[Energy].enabled = false;
    }

    private void CalculateEnergy()
    {
        float NewEnergy = (float)Turns / 3f;
        if (NewEnergy < 1f)
        {
            Energy = 1;
        }
        else
        {
            Energy = Mathf.RoundToInt(NewEnergy);
        }
    }
}
