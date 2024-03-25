using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCard : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer BackgroundCombatSprite;
    [SerializeField]
    private SpriteRenderer CombatSprite;


    [SerializeField]
    private List<SpriteRenderer> AttackPoints;

    [SerializeField]
    private List<SpriteRenderer> DefensePoints;

    [SerializeField]
    private List<SpriteRenderer> EnergyPoints;

    [SerializeField] private TextMeshPro NameOfCard;
    [SerializeField] private TextMeshPro HealthText;
    //[SerializeField] private TextMeshProUGUI DialogText;

    private int HealthPoints;
    private int Damage = 0;
    private int Armor = 0;
    private int Energy = 0;
    private int Turns = 0;
    private CombatTypes CombatType;

    private string InitialText;
    private string EffectiveText;
    private string NonEffectiveText;

    public void SetDataCard(CombatCardTemplate DataCard)
    {
        BackgroundCombatSprite.sprite = DataCard.BackgroundSprite;
        CombatSprite.sprite = DataCard.CardSprite;

        NameOfCard.text = DataCard.NameOfCard;
        HealthPoints = DataCard.HealthPoints;
        Damage = DataCard.Damage;
        Armor = DataCard.Armor;
        Turns = DataCard.Turns;
        Energy = CombatUtils.CalculateEnergy(Turns);
        CombatType = DataCard.CombatType;

        InitialText = DataCard.InitialText;
        EffectiveText = DataCard.EffectiveText;
        NonEffectiveText = DataCard.NonEffectiveText;

        //Encendemos los puntos de cada stat
        SetStat(Damage, AttackPoints);
        SetStat(Armor, DefensePoints);
        SetStat(Energy, EnergyPoints);
        HealthText.text = HealthPoints.ToString();
    }

    private void SetStat(int value, List<SpriteRenderer> sprites)
    {
        for (int i = 0; i < value && i < sprites.Count; i++)
        {
            sprites[i].enabled = true;
        }
    }

    public void ReduceEnergy()
    {
        Energy--;
        EnergyPoints[Energy].enabled = false;
    }

    public float GetCardWidth()
    {
        return BackgroundCombatSprite.bounds.size.x;
    }

    public CombatTypes GetCombatType()
    {
        return CombatType;
    }

    public int GetDamage()
    {
        return Damage;
    }

    public int GetArmor()
    {
        return Armor;
    }

    public void ReduceHealthPoints(int PointsToReduce)
    {
        HealthPoints = (HealthPoints - PointsToReduce) < 0 ? 0 : HealthPoints - PointsToReduce;
        HealthText.text = HealthPoints.ToString();
    }

    public int GetHealthPoints()
    {
        return HealthPoints;
    }

    public int GetCardEnergy()
    {
        return Energy;
    }
}
