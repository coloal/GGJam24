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
    private int HealthPoints;
    private int Damage = 0;
    private int Armor = 0;
    private CombatTypes CombatType;

    override public void SetDataCard(CardTemplate DataCard)
    {
        base.SetDataCard(DataCard);

        HealthPoints = DataCard.CombatInfo.HealthPoints;
        Damage = DataCard.CombatInfo.Damage;
        Armor = DataCard.CombatInfo.Armor;
        CombatType = DataCard.CombatInfo.CombatType;
    }


}
