using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyMember
{
    public CombatCardTemplate CombatCardTemplate;
    public int CurrentHealthPoints;
    public int CurrentEnergyPoints;

    public PartyMember(CombatCardTemplate CombatCardTemplate)
    {
        this.CombatCardTemplate = CombatCardTemplate;
        CurrentHealthPoints = CombatCardTemplate.HealthPoints;
        CurrentEnergyPoints = CombatUtils.CalculateEnergy(CombatCardTemplate.Turns);
    }
}
