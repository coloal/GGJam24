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

    public PartyMember(CombatCardTemplate combatCardTemplate)
    {
        CombatCardTemplate = combatCardTemplate;
        CurrentHealthPoints = combatCardTemplate.HealthPoints;
        CurrentEnergyPoints = CombatUtils.CalculateEnergy(combatCardTemplate.Turns);
    }

    public PartyMember(CombatCardTemplate combatCardTemplate, int healthPoints, int energyPoints)
    {
        CombatCardTemplate = combatCardTemplate;
        CurrentHealthPoints = healthPoints;
        CurrentEnergyPoints = energyPoints;
    }
}
