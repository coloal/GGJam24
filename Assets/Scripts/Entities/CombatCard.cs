using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] private TextMeshPro nameOfCard;
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private SpriteRenderer backgroundCombatSprite;
    [SerializeField] private SpriteRenderer combatSprite;

    [Header("Overlay text configurations")]
    [SerializeField] GameObject overlayTextContainer;
    [SerializeField] TextMeshProUGUI overlayTextMesh;
    [SerializeField] string leftSwipeWarningText;
    [SerializeField] string rightSwipeWarningText;
    [SerializeField] string topSwipeWarningText;

    [Header("Combat stats configurations")]
    [SerializeField] private List<SpriteRenderer> attackPoints;
    [SerializeField] private List<SpriteRenderer> defensePoints;

    [SerializeField] private List<SpriteRenderer> energyPoints;

    private int healthPoints;
    private int damage = 0;
    private int armor = 0;
    private int energy = 0;
    private int turns = 0;
    private CombatTypes combatType;

    private string initialText;
    private string superEffectiveText;
    private string notVeryEffectiveText;

    public void SetDataCard(CombatCardTemplate DataCard)
    {
        backgroundCombatSprite.sprite = DataCard.BackgroundSprite;
        combatSprite.sprite = DataCard.CardSprite;

        nameOfCard.text = DataCard.NameOfCard;
        healthPoints = DataCard.HealthPoints;
        damage = DataCard.Damage;
        armor = DataCard.Armor;
        turns = DataCard.Turns;
        energy = CombatUtils.CalculateEnergy(turns);
        combatType = DataCard.CombatType;

        initialText = DataCard.InitialText;
        superEffectiveText = DataCard.EffectiveText;
        notVeryEffectiveText = DataCard.NonEffectiveText;

        //Encendemos los puntos de cada stat
        SetStat(damage, attackPoints);
        SetStat(armor, defensePoints);
        SetStat(energy, energyPoints);
        healthText.text = healthPoints.ToString();
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
        energy--;
        energyPoints[energy].enabled = false;
    }

    public float GetCardWidth()
    {
        return backgroundCombatSprite.bounds.size.x;
    }

    public CombatTypes GetCombatType()
    {
        return combatType;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetArmor()
    {
        return armor;
    }

    public void ReduceHealthPoints(int PointsToReduce)
    {
        healthPoints = (healthPoints - PointsToReduce) < 0 ? 0 : healthPoints - PointsToReduce;
        healthText.text = healthPoints.ToString();
    }

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public int GetCardEnergy()
    {
        return energy;
    }

    public void EnableTopSwipeWarningText()
    {
        overlayTextMesh.text = topSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void EnableLeftSwipeWarningText()
    {
        overlayTextMesh.text = leftSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void EnableRightSwipeWarningText()
    {
        overlayTextMesh.text = rightSwipeWarningText;
        overlayTextContainer.SetActive(true);
    }

    public void DisableWarningText()
    {
        overlayTextContainer.SetActive(false);
    }
}
